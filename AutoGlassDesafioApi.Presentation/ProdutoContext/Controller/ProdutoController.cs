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

        private async Task<IActionResult> Salvar(ProdutoInput model)
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
        public async Task<IActionResult> RetornarPorCodigo([FromBody] ParamCodigoInput paramCodigoInput)
        {
            var retorno = await _serviceApplicationProduto.RetornarPorCodigoAsync(paramCodigoInput.Codigo);
            if (retorno == null)            
                return BadRequest("Produto não encontrado");
            
            return new JsonResult(retorno);
        }

        [HttpPost("RetornarVarios")]
        [ProducesResponseType(typeof(ProdutoOutput[]), 200)]
        public async Task<IActionResult> RetornarVarios(FilterProdutoInput filtros)
        {
            var retorno = await _serviceApplicationProduto.ListarAsync(filtros);

            if (_serviceApplicationProduto.RetornarNotificacao().RetornarErros().Count() != 0)            
                return BadRequest(error: String.Join(',', _serviceApplicationProduto.RetornarNotificacao().RetornarErros().ToArray()));            

            return new JsonResult(retorno);
        }

        [HttpGet("teste")]
        public async Task<IActionResult> teste()
        {
            return Ok();
        }

        [HttpPost("Incluir")]        
        public async Task<IActionResult> Incluir([FromBody] ProdutoInput produtoInput)
        {
            return await Salvar(produtoInput);            
        }

        [HttpPost("incluir2")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> incluir2([FromBody] int id)
        {
            return Ok(id);
        }

        [HttpPut("Editar")]
        public async Task<IActionResult> Editar([FromBody] ProdutoInput model)
        {
            if (model.Id == 0)            
                return BadRequest("Obrigatório informar o Id do Produto.");            

            return await Salvar(model);
        }

        [HttpDelete("Excluir")]
        public async Task<IActionResult> Excluir([FromBody] ParamIdInput ParamIdModel)
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
