using AutoGlassDesafioApi.Application.ProdutoContext.Models;
using AutoGlassDesafioApi.Domain.ProdutosContext.Filtros;
using AutoGlassDesafioApi.Domain.ProdutosContext.Models;
using AutoGlassDesafioApi.Presentation.ProdutoContext.Models;
using AutoGlassDesafioApi.Presentation.SharedContext.Models;
using Microsoft.AspNetCore.Mvc;
using Refit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.ProdutoContext.Interfaces
{
    public interface IProdutoAPI
    {        
        [Post("/api/Produto/RetornarPorCodigo")]
        Task<ApiResponse<ProdutoOutput>> RetornarPorCodigo([Body] ParamCodigoInput paramCodigoInput);
        [Post("/api/Produto/RetornarVarios")]
        Task<ApiResponse<ProdutoOutput[]>> RetornarVarios([Body] FilterProdutoInput filtros);        
        [Post("/api/Produto/Incluir")]        
        Task <ApiResponse<IActionResult>> Incluir([Body] ProdutoInput produtoInput);
        [Put("/api/Produto/Editar")]
        Task<ApiResponse<IActionResult>> Editar([Body] ProdutoInput model);
        [Delete("/api/Produto/Excluir")]
        Task<ApiResponse<IActionResult>> Excluir([Body] ParamIdInput ParamIdModel);
    }
}
