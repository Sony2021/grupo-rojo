using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using grupo_rojo.Data;
using grupo_rojo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace grupo_rojo.Controllers.Rest
{
    [ApiController]
    [Route("api/producto")]
    public class ProductoApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductoApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Producto>>> List()
        {
            var productos = await _context.DataProducto.ToListAsync();
             if(productos == null)
                return NotFound();
            return Ok(productos);
        }

    }
}