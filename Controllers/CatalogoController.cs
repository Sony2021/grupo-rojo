using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using grupo_rojo.Data;
using Microsoft.EntityFrameworkCore;
using grupo_rojo.Models;
using Microsoft.AspNetCore.Identity;

namespace grupo_rojo.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly ILogger<CatalogoController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CatalogoController(ILogger<CatalogoController> logger, ApplicationDbContext context,UserManager<IdentityUser> userManager) 

        {
            _logger = logger;
            _context = context;
            _userManager = userManager; 
        }

        public IActionResult Index(string? searchString)
        {
            // Obtener todos los productos de la base de datos
            var productos = _context.DataProducto.ToList();
            
            // Verificar si la lista de productos está vacía
            if (productos == null || productos.Count == 0)
            {
                return NotFound(); // Puedes manejar este caso según tus necesidades
            }
             // Aplicar filtro de búsqueda si el parámetro searchString no está vacío
            if (!String.IsNullOrEmpty(searchString))
            {
                productos = productos
                    .Where(s => s.Articulo.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }
            return View(productos); // Pasar la lista de productos a la vista
        }

        public async Task<IActionResult> Details(int? id){
            Producto objProduct = await _context.DataProducto.FindAsync(id);
            if(objProduct == null){
                return NotFound();
            }
            return View(objProduct);
        }

        public async Task<IActionResult> Add(int? id){
        var userID = _userManager.GetUserName(User);
            if(userID == null){
                ViewData["Message"] = "Por favor debe loguearse antes de agregar un producto";
                List<Producto> productos = new List<Producto>();
                return  View("Index",productos);
            }else{
                var producto = await _context.DataProducto.FindAsync(id);
                Util.SessionExtensions.Set<Producto>(HttpContext.Session,"MiUltimoProducto", producto);
                
                Proforma proforma = new Proforma();
                proforma.Producto = producto;
                proforma.Precio = producto.Precio;
                proforma.Cantidad = 1;
                proforma.UserID = userID;
                _context.Add(proforma);
                await _context.SaveChangesAsync();
                ViewData["Message"] = "Se Agrego al carrito";
            return RedirectToAction(nameof(Index));
            }
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}