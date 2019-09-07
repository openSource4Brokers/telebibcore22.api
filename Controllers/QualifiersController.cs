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
    public class QualifiersController : Controller
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public QualifiersController(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> GetQualifiers()
        {
            var qualifiers = await _context.TbQualifiers.ToListAsync();
            var qualifiersToReturn = _mapper.Map<IEnumerable<TbQualifierForIODto>>(qualifiers);
            return Ok(qualifiersToReturn);
        }

        // GET api/values/5
        [HttpGet("{d},{q}")]
        public async Task<IActionResult> GetQualifier(string d, string q)
        {
            // var qualifier = await _context.TbQualifiers.FirstOrDefaultAsync(x => x.DE == de);

            var qualifier = await _context.TbQualifiers
               .Where(dbD => dbD.DE == d)
               .Where(dbQ => dbQ.Qualifier == q)
               .ToListAsync();
                        
            return Ok(qualifier);
        }
    }
}
