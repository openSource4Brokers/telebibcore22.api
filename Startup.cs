using System.Text;
using AutoMapper;
using telebibcore22.api.Data;
using telebibcore22.api.Helpers;
using telebibcore22.api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace telebibcore22.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // IMPORTANT!!! FOR USE SQLSERVER 2005 - 2008>
            // add: , opt => opt.UseRowNumberForPaging()
            /* services.AddDbContext<DataContext>(options => options
                .UseSqlServer(Configuration.GetConnectionString("marDB"))
                .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.IncludeIgnoredWarning))); */

            // USE MYSQL:
            /* services.AddDbContext<DataContext>(x => x
                .UseMySql(Configuration.GetConnectionString("marDB"))
                .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.IncludeIgnoredWarning))); */

            // MOST USED COMMANDS:
            // dotnet restore
            // git push -u origin mastery
            // dotnet ef migrations add InitAllTables
            // dotnet ef database update

            // dotnet ef database drop
            // dotnet ef migrations remove

            // dotnet publish -c Release -r win7-x64
            // or dotnet publish -c Release (see also cproj file changes for compilation)
            //  dotnet build --runtime ubuntu.16.04-x64

            // MultipleActiveResultSets=True;

            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value);

            // services.AddDbContext<DataContext>(x => x.UseSqlite("Connectionstring"));
            services.AddDbContext<DataContext>(x => x
                            .UseSqlServer(Configuration.GetConnectionString("telebibDb22")));

            IdentityBuilder builder = services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<DataContext>();
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                options.AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin", "Moderator"));
                options.AddPolicy("VipOnly", policy => policy.RequireRole("VIP"));
            });

            services.AddMvc(options =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    options.Filters.Add(new AuthorizeFilter(policy));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.ReferenceLoopHandling =
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            services.AddCors();
            services.AddAutoMapper(typeof(AuthRepository).Assembly);
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.Configure<MailKitSettings>(Configuration.GetSection("MailKitSettings"));
            // Mapper.Reset();
            services.AddAutoMapper(typeof(CompanyRepository).Assembly);            
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();                        
            services.AddScoped<LogUserActivity>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseAuthentication();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Fallback", action = "Index" }
                );
            });
        }
    }
}
