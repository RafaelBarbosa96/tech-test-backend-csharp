using Microsoft.EntityFrameworkCore;
using Produto.Data;
using Produto.Models;
using Produto.Repositorios.Interfaces;

namespace Produto.Repositorios
{
    public class ProdutoRepositorioSQLServer : IProdutoRepositorioSQLServer
    {
        private readonly ProdutoDBContext _dbContext;
        public ProdutoRepositorioSQLServer(ProdutoDBContext produtoDBContext) 
        {
            _dbContext = produtoDBContext;
        }
        public async Task<ProdutoModel> BuscarProdutoPorId(int id)
        {
            var produto =  await _dbContext.Produto.FirstOrDefaultAsync(p => p.Id == id);
            if (produto == null)
            {
                throw new Exception($"Produto para o ID: {id} não foi encontrado no banco de dados.");
            }
            return produto;
        }

        public async Task<List<ProdutoModel>> BuscarProdutos()
        {
            var produtos = await _dbContext.Produto.ToListAsync();
            if (produtos.Count == 0)
                throw new Exception("Não tem nenhum produto cadastrado");
            return produtos;
        }

        public async Task<ProdutoModel> AdicionarProduto(ProdutoModel produto)
        {
            if (produto == null)
                throw new Exception("Produto deve estar vazio!");

            await _dbContext.Produto.AddAsync(produto);
            await _dbContext.SaveChangesAsync();

            return produto;
        }

        public async Task<ProdutoModel> AtualizarProduto(ProdutoModel produto, int id)
        {
            ProdutoModel produtoPorId = await BuscarProdutoPorId(id);

            if(produtoPorId == null) 
            {
                throw new Exception($"Produto para o ID: {id} não foi encontrado no banco de dados.");
            }

            produtoPorId.Nome = produto.Nome;
            produtoPorId.Preco = produto.Preco;
            produtoPorId.QuantidadeEstoque = produto.QuantidadeEstoque;

            _dbContext.Produto.Update(produtoPorId);
            await _dbContext.SaveChangesAsync();

            return produtoPorId;
            
        }


        public async Task<bool> DeletarProduto(int id)
        {
            ProdutoModel produtoPorId = await BuscarProdutoPorId(id);

            if (produtoPorId == null)
            {
                throw new Exception($"Produto para o ID: {id} não foi encontrado no banco de dados.");
            }

            _dbContext.Produto.Remove(produtoPorId);
            await _dbContext.SaveChangesAsync();

            return true;
        }

    }
}
