using AutoGlassDesafioApi.Domain.ProdutosContext.DTO;
using AutoGlassDesafioApi.Domain.SharedContext.DTO;
using AutoGlassDesafioApi.Domain.SharedContext.Interfaces.EF;
using AutoGlassDesafioApi.Domain.SharedContext.Interfaces.Notifications;
using Microsoft.Extensions.Logging;
using Optional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Domain.SharedContext.Services
{
    public abstract class ServiceBasic<T> : IServiceBasic<T> where T : EntityBasic
    {
        private readonly IRepository<T> _repositorio;
        protected INotification Notificacao { get { return _notificacao; } }
        protected INotification _notificacao;
        private readonly ILogger _logger;
        public ServiceBasic(IRepository<T> repositorio, INotification notificacao, ILogger<ServiceBasic<T>> logger)
        {
            _repositorio = repositorio;
            _notificacao = notificacao;
            _logger = logger;
        }
        public virtual void Excluir(T entidade)
        {
            _repositorio.Excluir(entidade);
            _logger.LogInformation(1001, "Exclusão {ID}", entidade.Id);
        }
        public INotification RetornarNotificacao()
        {
            return Notificacao;
        }
        public async Task<Option<T>> RetornarPorIdAsync(int id)
        {
            return await _repositorio.RetornarPorIdAsync(id);
        }

        public async Task<Option<T>> RetornarPorExpressionAsync(Expression<Func<T, bool>> predicate)
        {
            return await _repositorio.RetornarPorExpressionAsync(predicate);
        }

        public async Task<Option<IQueryable<T>>> RetornarVariosAsync()
        {
            return await _repositorio.RetornarVariosAsync();
        }
        public virtual async Task ValidarAsync(T entidade)
        {
            
        }
        public async void Salvar(T entidade)
        {
            await ValidarAsync(entidade);

            if (_notificacao.IsValid())
            {                
                _repositorio.Salvar(entidade);
                _logger.LogInformation(1002, "Salvar {ID}", entidade.Id);
            }
        }
    }
}
