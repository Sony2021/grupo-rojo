using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using grupo_rojo.Data;
using Microsoft.EntityFrameworkCore;

namespace grupo_rojo.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly ILogger<CatalogoController> _logger;
        private readonly ApplicationDbContext _context;

        public CatalogoController(ILogger<CatalogoController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Obtener todos los productos de la base de datos
            var productos = _context.DataProducto.ToList();
            
            // Verificar si la lista de productos está vacía
            if (productos == null || productos.Count == 0)
            {
                return NotFound(); // Puedes manejar este caso según tus necesidades
            }
            return View(productos);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}