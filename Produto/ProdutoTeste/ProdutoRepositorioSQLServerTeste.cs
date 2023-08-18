using Microsoft.EntityFrameworkCore;
using Produto.Data;
using Produto.Models;
using Produto.Repositorios;

namespace ProdutoTeste
{
    public class ProdutoRepositorioSQLServerTeste
    {        
        private readonly DbContextOptions<ProdutoDBContext> _options;

        public ProdutoRepositorioSQLServerTeste()
        {
            _options = new DbContextOptionsBuilder<ProdutoDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public async Task BuscarProdutoPorId_DeveRetornarNullQuandoNaoExistir()
        {

            using (var dbContext = new ProdutoDBContext(_options))
            {
                var produtoRepositorio = new ProdutoRepositorioSQLServer(dbContext);
                                
                var resultado = await Assert.ThrowsAsync<Exception>(
                    () => produtoRepositorio.BuscarProdutoPorId(1));
                Assert.Equal($"Produto para o ID: {1} não foi encontrado no banco de dados.", resultado.Message);                
            }
        }

        [Fact]
        public async Task BuscarProdutoPorId_DeveRetornarProdutoQuandoExistir()
        {
            using (var dbContext = new ProdutoDBContext(_options))
            {
                var produto = new ProdutoModel { Id = 1, Nome = "Produto de Teste", Preco = 10, QuantidadeEstoque = 5 };
                dbContext.Produto.Add(produto);
                dbContext.SaveChanges();

                var produtoRepositorio = new ProdutoRepositorioSQLServer(dbContext);
                                
                var resultado = await produtoRepositorio.BuscarProdutoPorId(1);
                                
                Assert.NotNull(resultado);                
            }
        }
       
        [Fact]
        public async Task BuscarProdutos_DeveRetornarListaQuandoExistiremProdutos()
        {

            using (var dbContext = new ProdutoDBContext(_options))
            {
                var produtos = new List<ProdutoModel>
            {
                new ProdutoModel { Id = 53, Nome = "Produto 1", Preco = 10, QuantidadeEstoque = 5  },
                new ProdutoModel {Id = 54, Nome = "Produto 2", Preco = 12, QuantidadeEstoque = 6}
            };
                dbContext.Produto.AddRange(produtos);
                dbContext.SaveChanges();

                var produtoRepositorio = new ProdutoRepositorioSQLServer(dbContext);

                var resultado = await produtoRepositorio.BuscarProdutos();

                Assert.Equal(produtos.Count, resultado.Count);                
            }
        }

        [Fact]
        public async Task BuscarProdutos_DeveRetornarNullQuandoNaoExistirProdutos()
        {

            using (var dbContext = new ProdutoDBContext(_options))
            {
                var produtos = new List<ProdutoModel>{};                

                var produtoRepositorio = new ProdutoRepositorioSQLServer(dbContext);

                var exception = await Assert.ThrowsAsync<Exception>(
                    async () => await produtoRepositorio.BuscarProdutos());
                Assert.Equal("Não tem nenhum produto cadastrado", exception.Message);
            }
        }

        [Fact]
        public async Task AdicionarProduto_DeveAdicionarProdutoNoBancoDeDados()
        {            
            using (var dbContext = new ProdutoDBContext(_options))
            {
                var produtoRepositorio = new ProdutoRepositorioSQLServer(dbContext);

                var produto = new ProdutoModel { Nome = "Novo Produto", Preco = 10, QuantidadeEstoque = 5 };

                var resultado = await produtoRepositorio.AdicionarProduto(produto);

                Assert.NotNull(resultado);
                Assert.Equal(1, dbContext.Produto.Count());
                Assert.Equal("Novo Produto", dbContext.Produto.First().Nome);
            }
        }

        [Fact]
        public async Task AdicionarProduto_DeveLancarExceptionQuandoProdutoForNulo()
        {
           using (var dbContext = new ProdutoDBContext(_options))
            {
                var produtoRepositorio = new ProdutoRepositorioSQLServer(dbContext);

                var resultado = await Assert.ThrowsAsync<Exception>(
                    () => produtoRepositorio.AdicionarProduto(null));
                Assert.Equal("Produto deve estar vazio!", resultado.Message);
            }
        }

        [Fact]
        public async Task AtualizarProduto_DeveAtualizarProdutoNoBancoDeDados()
        {
            using (var dbContext = new ProdutoDBContext(_options))
            {
                var produto = new ProdutoModel { Id = 1, Nome = "Produto Antigo", Preco = 6, QuantidadeEstoque = 3 };
                dbContext.Produto.Add(produto);
                dbContext.SaveChanges();

                var produtoRepositorio = new ProdutoRepositorioSQLServer(dbContext);

                var novoProduto = new ProdutoModel { Id = 1, Nome = "Produto Novo", Preco = 8, QuantidadeEstoque = 5 };

                var resultado = await produtoRepositorio.AtualizarProduto(novoProduto, 1);

                Assert.NotNull(resultado);
                Assert.Equal("Produto Novo", resultado.Nome);
                Assert.Equal(8, resultado.Preco);
                Assert.Equal(5, resultado.QuantidadeEstoque);
            }
        }

        [Fact]
        public async Task AtualizarProduto_DeveLancarExceptionQuandoProdutoNaoExistir()
        {
            using (var dbContext = new ProdutoDBContext(_options))
            {
                var produtoRepositorio = new ProdutoRepositorioSQLServer(dbContext);

                var produto = new ProdutoModel { Id = 1, Nome = "Produto Antigo", Preco = 6, QuantidadeEstoque = 3 };
                dbContext.Produto.Add(produto);
                dbContext.SaveChanges();

                var novoProduto = new ProdutoModel { Id = 2, Nome = "Produto Novo", Preco = 8, QuantidadeEstoque = 5 };

                var resultado = await Assert.ThrowsAsync<Exception>(
                    () => produtoRepositorio.AtualizarProduto(novoProduto, 2));
                Assert.Equal($"Produto para o ID: {novoProduto.Id} não foi encontrado no banco de dados.", resultado.Message);
            }
        }

        [Fact]
        public async Task DeletarProduto_DeveDeletarProdutoDoBancoDeDados()
        {
            using (var dbContext = new ProdutoDBContext(_options))
            {
                var produto = new ProdutoModel { Id = 1, Nome = "Produto Para Deletar", Preco = 10, QuantidadeEstoque = 2 };
                dbContext.Produto.Add(produto);
                dbContext.SaveChanges();

                var produtoRepositorio = new ProdutoRepositorioSQLServer(dbContext);

                var resultado = await produtoRepositorio.DeletarProduto(1);

                Assert.True(resultado);
                Assert.Empty(dbContext.Produto);
            }
        }

        [Fact]
        public async Task DeletarProduto_DeveLancarExceptionQuandoProdutoNaoExistir()
        {
            using (var dbContext = new ProdutoDBContext(_options))
            {
                var produtoRepositorio = new ProdutoRepositorioSQLServer(dbContext);

                var produto = new ProdutoModel { Id = 1, Nome = "Produto Para Deletar", Preco = 10, QuantidadeEstoque = 2 };
                dbContext.Produto.Add(produto);
                dbContext.SaveChanges();

                var resultado = await Assert.ThrowsAsync<Exception>(
                    () => produtoRepositorio.DeletarProduto(2));
                Assert.Equal($"Produto para o ID: {2} não foi encontrado no banco de dados.", resultado.Message);
            }
        }
    }
}
