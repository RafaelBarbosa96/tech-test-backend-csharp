using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Produto.Data;
using Produto.Models;
using Produto.Repositorios.Interfaces;

namespace Produto.Repositorios
{
    public class ProdutoRepositorioMongoDB : IProdutoRepositorioMongoDB
    {
        private readonly IProdutoContextMDB _context;

        public ProdutoRepositorioMongoDB(IProdutoContextMDB context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task AdicionarProdutoMDB(ProdutoModelMDB produto)
        {
            if (produto == null)
                throw new Exception("Produto deve estar vazio!");

            await _context.Produtos.InsertOneAsync(produto);
        }

        public async Task<bool> AtualizarProdutoMDB(ProdutoModelMDB produto)
        {
            var updateResult = await _context.Produtos.ReplaceOneAsync(
                filter: g => g.Id == produto.Id, replacement: produto);
            if (updateResult.MatchedCount == 0)
            {
                throw new Exception($"Produto para o ID: {produto.Id} não foi encontrado no banco de dados.");
            }

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<ProdutoModelMDB> BuscarProdutoMDBPorId(string id)
        {
            var busca =  await _context.Produtos.Find(p => p.Id == id).FirstOrDefaultAsync();
            if (busca == null)
            {
                throw new Exception($"Produto para o ID: {id} não foi encontrado no banco de dados.");
            }
            return busca;
        }

        public async Task<IEnumerable<ProdutoModelMDB>> BuscarProdutosMDB()
        {
            var produtos =  await _context.Produtos.Find(p => true).ToListAsync();
            if (produtos.Count == 0)
                throw new Exception("Não tem nenhum produto cadastrado");
            return produtos;
        }

        public async Task<bool> DeletarProdutoMDB(string id)
        {
            FilterDefinition<ProdutoModelMDB> filtro = Builders<ProdutoModelMDB>.Filter.Eq(p => p.Id, id);

            if (filtro == null)
            {
                throw new Exception($"Produto para o ID: {id} não foi encontrado no banco de dados.");
            }

            DeleteResult deleteResult = await _context.Produtos.DeleteOneAsync(filtro);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
    }
}
