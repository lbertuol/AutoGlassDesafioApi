using AutoGlassDesafioApi.Application.ProdutoContext.Interfaces;
using AutoGlassDesafioApi.Application.ProdutoContext.Models;
using AutoGlassDesafioApi.Application.SharedContext.Services;
using AutoGlassDesafioApi.Domain.ProdutosContext.DTO;
using AutoGlassDesafioApi.Domain.ProdutosContext.Enum;
using AutoGlassDesafioApi.Domain.ProdutosContext.Filtros;
using AutoGlassDesafioApi.Domain.ProdutosContext.Interfaces.Dapper;
using AutoGlassDesafioApi.Domain.ProdutosContext.Models;
using AutoGlassDesafioApi.Domain.ProdutosContext.Services;
using AutoGlassDesafioApi.Domain.SharedContext.Interfaces.Transaticon;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Application.ProdutoContext.Services
{    
    public class ServiceApplicationProduto : ServiceApplicationBasic<Produto, ProdutoInput, ProdutoOutput, FilterProdutoInput>,
                                          IServiceApplicationProduto
    {
        private IServiceProduto _serviceProduto;
        private readonly ITransaction _transaction;
        private readonly IMapper _mapper;
        private readonly IRepositoryProdutoRead _repositoryProdutoRead;

        public ServiceApplicationProduto(IServiceProduto servicoProduto, ITransaction transacao, IRepositoryProdutoRead repositoryProdutoRead, 
            IMapper mapper) : base(servicoProduto, transacao, mapper, repositoryProdutoRead)
        {
            _serviceProduto = servicoProduto;
            _transaction = transacao;
            _mapper = mapper;
            _repositoryProdutoRead = repositoryProdutoRead;
        }

        public async Task<ProdutoOutput> RetornarPorCodigoAsync(int codigo)
        {
            var consulta = await _serviceProduto.RetornarPorExpressionAsync(x => x.Codigo == codigo);

            Produto entidade = consulta.Match(
                some: retorno => retorno,
                none: () => {
                    _serviceProduto.RetornarNotificacao().Adicionar("Registro não encontrado.");
                    return default(Produto);
                }
            );
            return _mapper.Map<Produto, ProdutoOutput>(entidade);
        }       
    }
}
