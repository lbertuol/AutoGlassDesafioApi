using AutoGlassDesafioApi.Application.SharedContext.Interfaces;
using AutoGlassDesafioApi.Application.SharedContext.Models;
using AutoGlassDesafioApi.Domain.SharedContext.DTO;
using AutoGlassDesafioApi.Domain.SharedContext.Filters;
using AutoGlassDesafioApi.Domain.SharedContext.Interfaces.Dapper;
using AutoGlassDesafioApi.Domain.SharedContext.Interfaces.Notifications;
using AutoGlassDesafioApi.Domain.SharedContext.Interfaces.Transaticon;
using AutoGlassDesafioApi.Domain.SharedContext.Models;
using AutoGlassDesafioApi.Domain.SharedContext.Services;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Application.SharedContext.Services
{    
    public abstract class ServiceApplicationBasic<T, In, Out, InFiltro> : IServiceApplicationBasic<T, In, Out, InFiltro>
        where T : EntityBasic
        where In : InputBasic
        where Out : OutputBasic
        where InFiltro : FilterBasicInput
    {
        private IServiceBasic<T> _serviceBasic;
        private readonly ITransaction _transaction;
        private readonly IMapper _mapper;
        private readonly IRepositoryBaseDapper<Out, InFiltro> _repositoryBaseRead;
        private readonly ILogger _logger;

        public ServiceApplicationBasic(IServiceBasic<T> serviceBasic,
            ITransaction transaction, IMapper mapper,
            IRepositoryBaseDapper<Out, InFiltro> repositoryBaseRead)
        {
            _serviceBasic = serviceBasic;
            _transaction = transaction;
            _mapper = mapper;
            _repositoryBaseRead = repositoryBaseRead;
        }

        public async Task<Out> RetornarPorIdAsync(int id)
        {
            var consulta = await _serviceBasic.RetornarPorIdAsync(id);

            T entidade = consulta.Match(
                some: retorno => retorno,
                none: () => {
                    _serviceBasic.RetornarNotificacao().Adicionar("Registro não encontrado.");
                    return default(T);
                }
            );
            return _mapper.Map<T, Out>(entidade);
        }

        public async Task<IEnumerable<Out>> RetornarVariosAsync()
        {
            var consulta = await _serviceBasic.RetornarVariosAsync();

            IEnumerable<T> entidades = consulta.Match(
                some: retorno => retorno,
                none: () => {
                    _serviceBasic.RetornarNotificacao().Adicionar("Nenhum registro encontrado.");
                    return Enumerable.Empty<T>();
                }
            );
            return _mapper.Map<IEnumerable<T>, IEnumerable<Out>>(entidades);
        }

        public async Task<IEnumerable<Out>> ListarAsync(InFiltro filtros)
        {
            var consulta = await _repositoryBaseRead.ListarAsync(filtros);

            return consulta.Match(
                some: retorno => retorno,
                none: () => {
                    _serviceBasic.RetornarNotificacao().Adicionar("Nenhum registro encontrado.");
                    return Enumerable.Empty<Out>();
                }
            );
        }

        public async Task ExcluirAsync(int id)
        {
            try
            {
                var consulta = await _serviceBasic.RetornarPorIdAsync(id);

                T entidade = consulta.Match(
                    some: retorno => retorno,
                    none: () => {
                        return default(T);
                    }
                );

                _serviceBasic.Excluir(entidade);
                await _transaction.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _serviceBasic.RetornarNotificacao().Adicionar(ex.Message);
            }
        }
        public async Task SalvarAsync(In input)
        {
            try
            {
                T entidade = _mapper.Map<In, T>(input);

                if (entidade?.Id != 0)
                {
                    var consulta = await _serviceBasic.RetornarPorIdAsync(entidade.Id);

                    T _entidade = consulta.Match(
                        some: retorno => retorno,
                        none: () => {
                            _serviceBasic.RetornarNotificacao().Adicionar("Nenhum registro encontrado.");
                            return default(T);
                        }
                    );
                }

                _serviceBasic.Salvar(entidade);
                await _transaction.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _serviceBasic.RetornarNotificacao().Adicionar(ex.InnerException.Message);
            }
        }

        public INotification RetornarNotificacao()
        {
            return _serviceBasic.RetornarNotificacao();
        }

        public async void TransacaoWrapper(Action acao)
        {
            try
            {
                await _transaction.BeginTransactionAsync();
                acao();
                await _transaction.SaveChangesAsync();
                _transaction.Commit();
            }
            catch (Exception ex)
            {
                _transaction.Rollback();
                _serviceBasic.RetornarNotificacao().Adicionar(ex.InnerException.Message);
                _logger.LogError(ex.InnerException.Message);
            }
        }
    }
}
