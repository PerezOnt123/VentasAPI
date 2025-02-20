using VentasAPI.Models;

namespace VentasAPI.Data
{
    public class VentasContext : DbContext
    {
        public VentasContext(DbContextOptions<VentasContext> options)
            : base(options)
        {
        }

        public DbSet<Venta> Ventas { get; set; }
    }
}

