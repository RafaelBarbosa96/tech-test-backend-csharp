using Produto.Models;

namespace Produto.Repositorios.Interfaces
{
    public interface IProdutoRepositorioSQLServer
    {
        Task<List<ProdutoModel>> BuscarProdutos();
        Task<ProdutoModel> BuscarProdutoPorId(int id);
        Task<ProdutoModel> AdicionarProduto(ProdutoModel produto);
        Task<ProdutoModel> AtualizarProduto(ProdutoModel produto, int id);
        Task<bool> DeletarProduto(int id);
    }
}
