using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Produto.Data;
using Produto.Models;
using Produto.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdutoTeste
{
    public class ProdutoRepositorioMDBTeste
    {
        private readonly MongoDbRunner _runner;
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        public ProdutoRepositorioMDBTeste()
        {
            _runner = MongoDbRunner.Start();
            _client = new MongoClient(_runner.ConnectionString);
            _database = _client.GetDatabase("TestDatabase");
        }

        public void Dispose()
        {
            _runner.Dispose();
        }

        [Fact]
        public async Task AdicionarProdutoMDB_DeveAdicionarProdutoNoBancoDeDados()
        {
            var contextMock = new Mock<IProdutoContextMDB>();
            var collectionMock = new Mock<IMongoCollection<ProdutoModelMDB>>();

            contextMock.Setup(c => c.Produtos).Returns(collectionMock.Object);

            var produtoRepositorio = new ProdutoRepositorioMongoDB(contextMock.Object);
            var produto = new ProdutoModelMDB
            {
                Nome = "Produto de Teste",
                Preco = 9.99M
            };

            await produtoRepositorio.AdicionarProdutoMDB(produto);

            collectionMock.Verify(c => c.InsertOneAsync(produto, null, default), Times.Once);
        }

        [Fact]
        public async Task AdicionarProdutoMDB_DeveLancarExceptionQuandoProdutoNulo()
        {
            var contextMock = new Mock<IProdutoContextMDB>();
            var collectionMock = new Mock<IMongoCollection<ProdutoModelMDB>>();
            contextMock.Setup(c => c.Produtos).Returns(collectionMock.Object);

            var produtoRepositorio = new ProdutoRepositorioMongoDB(contextMock.Object);

            var resultado = await Assert.ThrowsAsync<Exception>(
                () => produtoRepositorio.AdicionarProdutoMDB(null));
            Assert.Equal("Produto deve estar vazio!", resultado.Message);
        }

        [Fact]
        public async Task AtualizarProdutoMDB_DeveAtualizarProdutoNoBancoDeDados()
        {
            var contextMock = new Mock<IProdutoContextMDB>();
            var collectionMock = new Mock<IMongoCollection<ProdutoModelMDB>>();
            contextMock.Setup(c => c.Produtos).Returns(collectionMock.Object);

            var produtoRepositorio = new ProdutoRepositorioMongoDB(contextMock.Object);
            var produto = new ProdutoModelMDB
            {
                Id = "123",
                Nome = "Produto Atualizado",
                Preco = 19.99M
            };

            var updateResult = new ReplaceOneResult.Acknowledged(1, 1, null);
            collectionMock.Setup(c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<ProdutoModelMDB>>(), produto, It.IsAny<ReplaceOptions>(), default)).ReturnsAsync(updateResult);

            var resultado = await produtoRepositorio.AtualizarProdutoMDB(produto);

            Assert.True(resultado);
            collectionMock.Verify(c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<ProdutoModelMDB>>(), produto, It.IsAny<ReplaceOptions>(), default), Times.Once);
        }

        [Fact]
        public async Task AtualizarProdutoMDB_DeveLancarExceptionQuandoProdutoNaoEncontrado()
        {
            var contextMock = new Mock<IProdutoContextMDB>();
            var collectionMock = new Mock<IMongoCollection<ProdutoModelMDB>>();
            contextMock.Setup(c => c.Produtos).Returns(collectionMock.Object);

            var produtoRepositorio = new ProdutoRepositorioMongoDB(contextMock.Object);
            var produto = new ProdutoModelMDB
            {
                Id = "123",
                Nome = "Produto Atualizado",
                Preco = 19.99M
            };

            var updateResult = new ReplaceOneResult.Acknowledged(0, 0, null);
            collectionMock.Setup(c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<ProdutoModelMDB>>(), produto, It.IsAny<ReplaceOptions>(), default)).ReturnsAsync(updateResult);

            var resultado = await Assert.ThrowsAsync<Exception>(
                () => produtoRepositorio.AtualizarProdutoMDB(produto));
            Assert.Equal($"Produto para o ID: {produto.Id} não foi encontrado no banco de dados.", resultado.Message);
        }

        [Fact]
        public async Task BuscarProdutosMDB_DeveLancarExceptionQuandoNenhumProdutoEncontrado()
        {
            var contextMock = new Mock<IProdutoContextMDB>();
            var collectionMock = new Mock<IMongoCollection<ProdutoModelMDB>>();
            contextMock.Setup(c => c.Produtos).Returns(collectionMock.Object);

            var produtoRepositorio = new ProdutoRepositorioMongoDB(contextMock.Object);

            var asyncCursor = new Mock<IAsyncCursor<ProdutoModelMDB>>();
            asyncCursor.Setup(c => c.Current).Returns(new List<ProdutoModelMDB>());
            asyncCursor.Setup(c => c.MoveNext(It.IsAny<CancellationToken>())).Returns(false);

            collectionMock.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<ProdutoModelMDB>>(),
                It.IsAny<FindOptions<ProdutoModelMDB, ProdutoModelMDB>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(asyncCursor.Object);

            var resultado = await Assert.ThrowsAsync<Exception>(
                () => produtoRepositorio.BuscarProdutosMDB());
            Assert.Equal("Não tem nenhum produto cadastrado", resultado.Message);
        }


    }
}
