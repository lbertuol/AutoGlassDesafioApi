using AutoGlassDesafioApi.Domain.ProdutosContext.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Domain.ProdutosContext.DTO
{
    public partial class Produto
    {
        public static Produto Create(
            int id,
            int codigo,
            string descricao,
            DateTime dataFabricacao,
            DateTime dataValidade,
            string fornecedorCodigo,
            string fornecedorDescricao,
            string fornecedorCNPJ
            ) =>
            new Produto(0)
            {
                Id = id,
                Codigo = codigo,
                Descricao = descricao,
                DataFabricacao = dataFabricacao,    
                DataValidade = dataValidade,
                FornecedorCodigo = fornecedorCodigo,
                FornecedorDescricao = fornecedorDescricao,
                FornecedorCNPJ = fornecedorCNPJ,
                Situacao = EnumSituacaoProduto.Ativo
            };
    }
}
