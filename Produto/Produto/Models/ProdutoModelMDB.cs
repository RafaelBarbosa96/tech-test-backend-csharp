using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Produto.Models
{
    public class ProdutoModelMDB
    {
        [BsonId]        
        public string Id { get; set; }
        [BsonElement("Nome")]
        public string Nome { get; set; }
        private decimal _preco;
        public decimal Preco
        {
            get { return _preco; }
            set
            {
                if (value < 0)
                {
                    throw new InvalidOperationException("Preço não pode ser negativo.");
                }
                _preco = value;
            }
        }
        public decimal QuantidadeEstoque { get; set; }
        
        public DateTime DataCriacao { get; set; }
        
        public decimal ValorTotal
        {
            get { return Preco * QuantidadeEstoque; }
        }
    }
}
