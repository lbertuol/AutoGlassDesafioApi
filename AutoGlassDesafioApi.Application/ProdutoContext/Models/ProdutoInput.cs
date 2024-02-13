using AutoGlassDesafioApi.Application.SharedContext.Models;
using AutoGlassDesafioApi.Domain.ProdutosContext.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Application.ProdutoContext.Models
{
    public class ProdutoInput: InputBasic
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public EnumSituacaoProduto Situacao { get; set; }
        public DateTime DataFabricacao { get; set; }
        public DateTime DataValidade { get; set; }
        public string FornecedorCodigo { get; set; }
        public string FornecedorDescricao { get; set; }
        public string FornecedorCNPJ { get; set; }
    }
}
