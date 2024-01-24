using Microsoft.EntityFrameworkCore;
using ShoppingCart.MVCAppExamples.Models;

namespace ShoppingCart.MVCAppExamples
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<ItemDataModel> Data { get; set; }
    }
}
