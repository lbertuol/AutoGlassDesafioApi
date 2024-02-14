using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using AutoGlassDesafioApi.Presentation.SharedContext.Controller;
using AutoGlassDesafioApi.Application.ProdutoContext.Interfaces;
using AutoGlassDesafioApi.Application.ProdutoContext.Models;
using System.Linq;
using AutoGlassDesafioApi.Domain.ProdutosContext.Models;
using AutoGlassDesafioApi.Domain.ProdutosContext.Filtros;
using AutoGlassDesafioApi.Presentation.SharedContext.Models;
using AutoGlassDesafioApi.Presentation.ProdutoContext.Models;

namespace AutoGlassDesafioApi.Presentation.ProdutoContext.Controller
{
    public class ProdutoController : BaseAbstractController
    {
        private readonly IServiceApplicationProduto _serviceApplicationProduto;
        public ProdutoController(IServiceApplicationProduto serviceApplicationProduto)
        {
            _serviceApplicationProduto = serviceApplicationProduto;
        }

        private async Task<IActionResult> SalvarAsync(ProdutoInput model)
        {
            try
            {
                await _serviceApplicationProduto.SalvarAsync(model);

                if (_serviceApplicationProduto.RetornarNotificacao().RetornarErros().Count() != 0)
                {
                    return BadRequest(error: String.Join(',', _serviceApplicationProduto.RetornarNotificacao().RetornarErros().ToArray()));
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(error: ex.Message + " - " +
                    ex.InnerException.Message);
            }
        }        

        [HttpPost("RetornarPorCodigo")]
        [ProducesResponseType(typeof(ProdutoOutput), 200)]        
        public async Task<IActionResult> RetornarPorCodigoAsync([FromBody] ParamCodigoInput paramCodigoInput)
        {
            var retorno = await _serviceApplicationProduto.RetornarPorCodigoAsync(paramCodigoInput.Codigo);
            if (retorno == null)            
                return BadRequest("Produto não encontrado");
            
            return new JsonResult(retorno);
        }

        [HttpPost("RetornarVarios")]
        [ProducesResponseType(typeof(ProdutoOutput[]), 200)]
        public async Task<IActionResult> RetornarVariosAsync(FilterProdutoInput filtros)
        {
            var retorno = await _serviceApplicationProduto.ListarAsync(filtros);

            if (_serviceApplicationProduto.RetornarNotificacao().RetornarErros().Count() != 0)            
                return BadRequest(error: String.Join(',', _serviceApplicationProduto.RetornarNotificacao().RetornarErros().ToArray()));            

            return new JsonResult(retorno);
        }
        
        [HttpPost("Incluir")]        
        public async Task<IActionResult> IncluirAsync([FromBody] ProdutoInput produtoInput)
        {
            return await SalvarAsync(produtoInput);            
        }        

        [HttpPut("Editar")]
        public async Task<IActionResult> EditarAsync([FromBody] ProdutoInput model)
        {
            if (model.Id == 0)            
                return BadRequest("Obrigatório informar o Id do Produto.");            

            return await SalvarAsync(model);
        }

        [HttpDelete("Excluir")]
        public async Task<IActionResult> ExcluirAsync([FromBody] ParamIdInput ParamIdModel)
        {
            try
            {
                await _serviceApplicationProduto.ExcluirAsync(ParamIdModel.Id);

                if (_serviceApplicationProduto.RetornarNotificacao().RetornarErros().Count() != 0)                
                    return BadRequest(error: String.Join(',', _serviceApplicationProduto.RetornarNotificacao().RetornarErros().ToArray()));                

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(error: ex.Message + " - " +
                    ex.InnerException.Message);
            }
        }
    }
}
