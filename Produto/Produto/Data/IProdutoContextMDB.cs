using MongoDB.Driver;
using Produto.Models;

namespace Produto.Data
{
    public interface IProdutoContextMDB
    {
        IMongoCollection<ProdutoModelMDB> Produtos { get; }
    }
}
