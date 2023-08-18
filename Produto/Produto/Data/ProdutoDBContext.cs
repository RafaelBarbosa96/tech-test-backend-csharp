using Microsoft.EntityFrameworkCore;
using Produto.Models;

namespace Produto.Data
{
    public class ProdutoDBContext : DbContext
    {
        public ProdutoDBContext()
        {
            
        }
        public ProdutoDBContext(DbContextOptions<ProdutoDBContext> options) : base(options)
        {}

        public DbSet<ProdutoModel> Produto { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProdutoMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
