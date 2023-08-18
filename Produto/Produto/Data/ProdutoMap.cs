using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Produto.Models;

namespace Produto.Data
{
    public class ProdutoMap : IEntityTypeConfiguration<ProdutoModel>
    {
        public void Configure(EntityTypeBuilder<ProdutoModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Nome).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Preco).IsRequired();
            builder.Property(x => x.QuantidadeEstoque).IsRequired();
            builder.Property(x => x.DataCriacao).IsRequired();
        }
    }
}