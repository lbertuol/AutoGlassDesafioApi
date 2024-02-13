using AutoGlassDesafioApi.Domain.ProdutosContext.Enum;
using AutoGlassDesafioApi.Domain.SharedContext.DTO;
using FluentValidator.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
            this.Id = Id;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Codigo { get; }
        public string Descricao { get; private set; }
        public EnumSituacaoProduto Situacao {  get; private set; }
        public DateTime DataFabricacao { get; private set; }
        public DateTime DataValidade { get; private set; }
        public string FornecedorCodigo { get; private set; }
        public string FornecedorDescricao { get; private set; }
        public string FornecedorCNPJ { get; private set; }

        public void AlterarSituacaoProduto(EnumSituacaoProduto enumSituacaoProduto) 
        { 
            this.Situacao = enumSituacaoProduto;
        }
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
                .IsGreaterThan(produto.DataValidade, produto.DataFabricacao, "Data Fabricação", "Data de Fabricação não pode ser maior ou igual que a data de Validade");            
        }
    }
}
