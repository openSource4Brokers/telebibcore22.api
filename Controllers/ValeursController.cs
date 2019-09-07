using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using telebibcore22.api.Data;
using telebibcore22.api.Dtos.Tb;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace telebibcore22.api.Controllers
{
    // [Authorize]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class ValeursController : Controller
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public ValeursController(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> GetValeurs()
        {
            var valeurs = await _context.TbValeurs.ToListAsync();
            var valeursToReturn = _mapper.Map<IEnumerable<TbValeurForIODto>>(valeurs);
            return Ok(valeursToReturn);
        }

        // GET api/values/5
        [HttpGet("{c},{v}")]
        public async Task<IActionResult> GetValeur(string c, string v)
        {
            var valeur = await _context.TbValeurs
               .Where(dbC => dbC.Code == c)
               .Where(dbV => dbV.Valeur == v)
               .ToListAsync();

            return Ok(valeur);
        }
    }
}
