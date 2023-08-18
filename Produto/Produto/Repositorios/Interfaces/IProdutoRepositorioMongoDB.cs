using Produto.Models;

namespace Produto.Repositorios.Interfaces
{
    public interface IProdutoRepositorioMongoDB
    {
        Task<IEnumerable<ProdutoModelMDB>> BuscarProdutosMDB();
        Task<ProdutoModelMDB> BuscarProdutoMDBPorId(string id);
        Task AdicionarProdutoMDB(ProdutoModelMDB produto);
        Task<bool> AtualizarProdutoMDB(ProdutoModelMDB produto);
        Task<bool> DeletarProdutoMDB(string id);
    }
}
