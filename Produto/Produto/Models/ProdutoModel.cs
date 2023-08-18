using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Produto.Models
{
    public class ProdutoModel
    {        
        public int Id { get; set; }        
        public string? Nome { get; set; }
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
        public int QuantidadeEstoque { get; set; }
        public DateTime DataCriacao { get; set; }
        public decimal ValorTotal 
        {
            get { return Preco * QuantidadeEstoque; }    
        } 
    }
}
 