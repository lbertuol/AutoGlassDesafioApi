using AutoGlassDesafioApi.Application.ProdutoContext.Models;
using AutoGlassDesafioApi.Application.SharedContext.Interfaces;
using AutoGlassDesafioApi.Domain.ProdutosContext.DTO;
using AutoGlassDesafioApi.Domain.ProdutosContext.Filtros;
using AutoGlassDesafioApi.Domain.ProdutosContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Application.ProdutoContext.Interfaces
{
    public interface IServiceApplicationProduto : IServiceApplicationBasic<Produto, ProdutoInput, ProdutoOutput, FilterProdutoInput>
    {
        Task<ProdutoOutput> RetornarPorCodigoAsync(int codigo);
    }
}
