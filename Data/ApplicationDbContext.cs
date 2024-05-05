using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace grupo_rojo.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<grupo_rojo.Models.Contacto> DataContacto {get; set;}
    public DbSet<grupo_rojo.Models.Producto> DataProducto {get; set; }
    public DbSet<grupo_rojo.Models.Proforma> DataItemCarrito {get; set; } 
}
