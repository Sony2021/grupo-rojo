using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using grupo_rojo.Data;
using grupo_rojo.Models;
using Microsoft.EntityFrameworkCore;

namespace grupo_rojo.Service
{
    public class ProductoService
    {
        private readonly ILogger<ProductoService> _logger;
        private readonly ApplicationDbContext _context;

        public ProductoService(ILogger<ProductoService> logger,ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<List<Producto>?> GetAll(){
            if(_context.DataProducto == null )
                return null;
            return await _context.DataProducto.ToListAsync();
        }
    }
}