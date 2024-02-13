using AutoGlassDesafioApi.Domain.ProdutosContext.DTO;
using AutoGlassDesafioApi.Domain.ProdutosContext.Enum;
using AutoGlassDesafioApi.Domain.ProdutosContext.Interfaces.Dapper;
using AutoGlassDesafioApi.Domain.ProdutosContext.Interfaces.EF;
using AutoGlassDesafioApi.Domain.SharedContext.Interfaces.Notifications;
using AutoGlassDesafioApi.Domain.SharedContext.Interfaces.Transaticon;
using AutoGlassDesafioApi.Domain.SharedContext.Services;
using Microsoft.Extensions.Logging;
using Optional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Domain.ProdutosContext.Services
{
    public class ServiceProduto : ServiceBasic<Produto>, IServiceProduto
    {
        private readonly IRepositoryProdutoWriteRead _repositoryProdutoWriteRead;
        private readonly IRepositoryProdutoRead _repositoryLeituraProdutoRead;
        
        public ServiceProduto(IRepositoryProdutoWriteRead repositoryProdutoWriteRead, IRepositoryProdutoRead repositoryLeituraProdutoRead,
            INotification notification, ILogger<ServiceProduto> logger)
            : base(repositoryProdutoWriteRead, notification, logger)
        {
            _repositoryProdutoWriteRead = repositoryProdutoWriteRead;
            _repositoryLeituraProdutoRead = repositoryLeituraProdutoRead;
        }

        public override async Task ValidarAsync(Produto produto)
        {
            var produtoValidacao = new ProdutoValidacao(produto);

            foreach (var notificacao in produtoValidacao.Contract.Notifications)
                _notificacao.Adicionar(notificacao.Message);            
        }

        public override async void Excluir(Produto entidade)
        {
            try
            {
                if (entidade == null)
                {
                    this.Notificacao.Adicionar("Nenhum registro encontrado.");
                    return;
                }                    

                entidade.AlterarSituacaoProduto(EnumSituacaoProduto.Inativo);                
                this.Salvar(entidade);
            }
            catch (Exception ex)
            {
                this.RetornarNotificacao().Adicionar(ex.Message);
            }
        }        
    }
}
