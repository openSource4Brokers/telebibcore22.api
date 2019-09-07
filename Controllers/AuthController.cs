using AutoMapper;

using MailKit.Net.Smtp;
using MimeKit;

using telebibcore22.api.Dtos.User;
using telebibcore22.api.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using telebibcore22.api.Helpers;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IOptions<MailKitSettings> _mailKitConfig;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(IConfiguration config,
            IOptions<MailKitSettings> mailKitConfig,
            IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _config = config;
            _mailKitConfig = mailKitConfig;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            var userToCreate = _mapper.Map<User>(userForRegisterDto);

            var result = await _userManager.CreateAsync(userToCreate, userForRegisterDto.Password);

            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(userToCreate, "Member").Wait();
            }

            var userToReturn = _mapper.Map<UserForDetailedDto>(userToCreate);

            if (result.Succeeded)
            {
                SendMailConfirmation(userToCreate);
                return CreatedAtRoute("GetUser",
                    new { controller = "Users", id = userToCreate.Id }, userToReturn);
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var user = await _userManager.FindByNameAsync(userForLoginDto.Username);

            var result = await _signInManager
                .CheckPasswordSignInAsync(user, userForLoginDto.Password, false);

            if (result.Succeeded)
            {
                var appUser = await _userManager.Users.Include(p => p.Photos)
                    .FirstOrDefaultAsync(u => u.NormalizedUserName == userForLoginDto.Username.ToUpper());

                var userToReturn = _mapper.Map<UserForListDto>(appUser);

                return Ok(new
                {
                    token = GenerateJwtToken(appUser).Result,
                    user = userToReturn
                });
            }

            return Unauthorized();
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddYears(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private void SendMailConfirmation(User newUser)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(
                _mailKitConfig.Value.SendMailAddress,
                _mailKitConfig.Value.SendMailAddress));

            message.To.Add(new MailboxAddress(
                newUser.UserName,
                newUser.Email));

            message.To.Add(new MailboxAddress(
                _mailKitConfig.Value.AdminCCFullName,
                _mailKitConfig.Value.AdminCCMailAddress));

            message.Subject = "Nieuwe gebruikersregistratie";

            var builder = new BodyBuilder();

            // Set the plain-text version of the message text
            builder.TextBody = @"Hallo, U registreerde zich zo-even voor de portfolio app (en webtoepassing rv-services.be).
                                Na controle van deze registratie, ontvangt U als klant mailbevestiging en toegang tot alle functies.
                                Controle op: Identiteitskaart rijksregister nummer: " + newUser.BerNumber +
                                " voor gebruikersnaam: " + newUser.UserName +
                                " met opgegeven email adres: " + newUser.Email +
                                " Groeten, Admin";

            // Set the html version of the message text
            builder.HtmlBody = string.Format(@"<p>Hallo,<br>
                <p>U registreerde zich zo-even voor de portfolio app (en webtoepassing rv-services.be).
                Na controle van deze registratie, ontvangt U als klant mailbevestiging en toegang tot alle functies.<br>
                <p>Controle op:<br> * Identiteitskaart rijksregister nummer: " + newUser.BerNumber +
                 "<br> * voor gebruikersnaam: " + newUser.UserName +
                 "<br> * met opgegeven email adres: " + newUser.Email + "<br><p>Groeten, Admin<br>");

            // HOWTO attach ??
            // builder.Attachments.Add(@"http://www.rv.be/docserver/!pdfDocumenten/mar/ODBC-MARDSN10-Instellen.pdf");

            // Now we just need to set the message body and we're done
            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(
                    _mailKitConfig.Value.SendMailUrl, 587, false);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(
                    _mailKitConfig.Value.SendMailAddress,
                    _mailKitConfig.Value.SendMailPassword);

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}