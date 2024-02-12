using AutoGlassDesafioApi.Domain.ProdutosContext.Enum;
using AutoGlassDesafioApi.Domain.SharedContext.DTO;
using FluentValidator.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Domain.ProdutosContext.DTO
{
    public partial class Produto: EntityBasic
    {
        private Produto()
        {

        }

        private Produto(int Id) : base(Id)
        {

        }

        public int Codigo { get; private set; }
        public string Descricao { get; private set; }
        public EnumSituacaoProduto Situacao {  get; private set; }
        public DateTime DataFabricacao { get; private set; }
        public DateTime DataValidade { get; private set; }
        public string FornecedorCodigo { get; private set; }
        public string FornecedorDescricao { get; private set; }
        public string FornecedorCNPJ { get; private set; }
    }

    public class ProdutoValidacao : IContract
    {
        public ValidationContract Contract { get; }

        public ProdutoValidacao(Produto produto)
        {
            Contract = new ValidationContract();
            Contract
                .Requires()
                .IsNotNullOrEmpty(produto.Descricao, "Descrição", "Informe a descrição do Produto!");
            Contract
                .Requires()
                .IsGreaterThan(produto.DataFabricacao, produto.DataValidade, "Data Fabricação", "Data de Fabricação não pode ser maior que a data de Validade");
            Contract
                .Requires()
                .IsLowerThan(produto.DataValidade, produto.DataFabricacao, "Data Validade", "Data de Validade não pode ser menor que a data de Fabricação");
        }
    }
}
