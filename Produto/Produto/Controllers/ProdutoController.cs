using Microsoft.AspNetCore.Mvc;
using Produto.Models;
using Produto.Repositorios.Interfaces;

namespace Produto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoRepositorioSQLServer _produtoRepositorio;
        private readonly IProdutoRepositorioMongoDB _produtoRepositorioMongoDb;

        public ProdutoController(IProdutoRepositorioSQLServer produtoRepositorio, IProdutoRepositorioMongoDB produtoRepositorioMongoDb)
        {
            _produtoRepositorio = produtoRepositorio ??
                throw new ArgumentNullException(nameof(produtoRepositorio));
            _produtoRepositorioMongoDb = produtoRepositorioMongoDb ??
                throw new ArgumentNullException(nameof(_produtoRepositorioMongoDb)); 
        }
        

        
        //Início dos Métodos SQL Server 
        [HttpGet("SQL Server")]
        public async Task<ActionResult<List<ProdutoModel>>> BuscarProdutos()
        {
            
            var produtos = await _produtoRepositorio.BuscarProdutos();

            if(!produtos.Any()) 
            {
                throw new InvalidOperationException("Não há nenhum produto cadastrado!");
            }            

            return produtos;
        }

        [HttpGet("SQL Server/{id}")]
        public async Task<ActionResult<ProdutoModel>> BuscarProdutoPorId(int id)
        {
            var produto = await _produtoRepositorio.BuscarProdutoPorId(id);
            
            if (produto == null)
            {
                throw new Exception($"Não há nenhum produto cadastrado com o id: {id}");
            }            

            return produto;
        }

        [HttpPost("SQL Server")]
        public async Task<ActionResult<ProdutoModel>> AdicionarProduto([FromBody] ProdutoModel produtoModel)
        {
            var produto = await _produtoRepositorio.AdicionarProduto(produtoModel);
            return produto;
        }

        [HttpPut("SQL Server/{id}")]
        public async Task<ActionResult<ProdutoModel>> AtualizarProduto([FromBody] ProdutoModel produtoAtualizado, int id)
        {
            
            produtoAtualizado.Id = id;
            var produto = await _produtoRepositorio.AtualizarProduto(produtoAtualizado, id);
            
            if (produto == null)
            {
                throw new Exception($"Não há nenhum produto cadastrado para ser atualizado com o id: {id}");
            }            

            return produto;
        }

        [HttpDelete("SQL Server/{id}")]
        public async Task<ActionResult<ProdutoModel>> DeletarProduto(int id)
        {
            bool deletado = await _produtoRepositorio.DeletarProduto(id);                        
            
            return Ok(deletado);
        }
        //Fim

        //Início dos Métodos MongoDB
        [HttpGet("MongoDB")]
        public async Task<ActionResult<IEnumerable<ProdutoModelMDB>>> BuscarProdutosMDB()
        {
            var produtos = await _produtoRepositorioMongoDb.BuscarProdutosMDB();
            if (!produtos.Any())
            {
                throw new InvalidOperationException("Não há nenhum produto cadastrado!");
            }

            return Ok(produtos);
        }

        [HttpGet("MongoDB/{id}")]
        public async Task<ActionResult<ProdutoModelMDB>> BuscarProdutoMDBPorId(string id)
        {
            var produto = await _produtoRepositorioMongoDb.BuscarProdutoMDBPorId(id);

            if (produto == null)
            {
                throw new Exception($"Não há nenhum produto cadastrado com o id: {id}");
            }

            return Ok(produto);
        }

        [HttpPost("MongoDB")]
        public async Task<ActionResult> AdicionarProdutoMDB(ProdutoModelMDB produtoModel)
        {
            await _produtoRepositorioMongoDb.AdicionarProdutoMDB(produtoModel);
            return Ok(produtoModel);
        }

        [HttpPut("MongoDB/{id}")]
        public async Task<ActionResult> AtualizarProduto(ProdutoModelMDB produtoAtualizado, string id)
        {
            if (id != produtoAtualizado.Id)
            {
                throw new Exception($"Não há nenhum produto cadastrado para ser atualizado com o id: {id}");
            }
            produtoAtualizado.Id = id;
            var produto = await _produtoRepositorioMongoDb.AtualizarProdutoMDB(produtoAtualizado);
                       

            return Ok(produto);
        }

        [HttpDelete("MongoDB/{id}")]
        public async Task<ActionResult<ProdutoModelMDB>> DeletarProdutoMDB(string id)
        {
            bool deletado = await _produtoRepositorioMongoDb.DeletarProdutoMDB(id);

            return Ok(deletado);
        }
        //Fim
    }
}
