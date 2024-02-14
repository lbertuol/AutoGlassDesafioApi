using AutoGlassDesafioApi.Domain.ProdutosContext.Enum;
using AutoGlassDesafioApi.Domain.SharedContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Domain.ProdutosContext.Models
{
    public class ProdutoOutput : OutputBasic
    {
        public int Id { get; set; }
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public EnumSituacaoProduto Situacao { get; set; }
        public DateTime DataFabricacao { get; set; }
        public DateTime DataValidade { get; set; }
        public string FornecedorCodigo { get; set; }
        public string FornecedorDescricao { get; set; }
        public string FornecedorCNPJ { get; set; }
    }

    public class ListaProdutoOutput : OutputBasic
    {
        public IEnumerable<ProdutoOutput> Produtos { get; set; }
        public int TotalRegistros { get; set; }
    }
}
