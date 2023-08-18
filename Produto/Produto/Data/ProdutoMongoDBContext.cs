using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Produto.Models;

namespace Produto.Data
{
    public class ProdutoMongoDBContext : IProdutoContextMDB
    {
        public ProdutoMongoDBContext(IConfiguration configuration) 
        {
            var client = new MongoClient(configuration.GetConnectionString(
                "MongoDbConnection"));
            var database = client.GetDatabase("ProdutoDB");
            Produtos = database.GetCollection<ProdutoModelMDB>("Produtos");

            
        }
        public IMongoCollection<ProdutoModelMDB> Produtos { get; }
    }

}
