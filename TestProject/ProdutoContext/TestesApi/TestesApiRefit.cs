using AutoGlassDesafioApi.Application.ProdutoContext.Models;
using AutoGlassDesafioApi.Domain.ProdutosContext.Enum;
using AutoGlassDesafioApi.Domain.ProdutosContext.Filtros;
using AutoGlassDesafioApi.Domain.ProdutosContext.Interfaces.EF;
using AutoGlassDesafioApi.Infrastructure.Database.ProdutosContext.Repository.Entity;
using AutoGlassDesafioApi.Infrastructure.Database.SharedContext;
using AutoGlassDesafioApi.Presentation.ProdutoContext.Models;
using AutoGlassDesafioApi.Presentation.SharedContext.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Refit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestProject.ProdutoContext.Interfaces;
using Xunit;

namespace TestProject.ProdutoContext.TestesApi
{
    public class TestesApiRefit
    {
        private readonly IProdutoAPI _produtoApi;
        private readonly DbContextOptions<ContextoPrincipal> _dbContextOptions;
        private ContextoPrincipal dbContext;        

        public TestesApiRefit()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ContextoPrincipal>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Usar um nome de banco de dados exclusivo para cada teste
                .Options;

            dbContext = new ContextoPrincipal(_dbContextOptions);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json")
                .AddEnvironmentVariables();
            var configuration = builder.Build();

            _produtoApi = RestService.For<IProdutoAPI>(
                new HttpClient()
                { BaseAddress = new Uri(configuration["UrlWebAppTestes"]) });            

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        [Fact]        
        public async Task TestarConsultaPorCodigo()
        {
            var model = new ParamCodigoInput
            {
                Codigo = 3
            };

            var response = await _produtoApi.RetornarPorCodigo(model);            
            
            response.StatusCode.Should().Be(HttpStatusCode.OK,
                $"* Ocorreu uma falha: Status Code esperado (200, OK) diferente do resultado gerado *");

            var resultado = response.Content;
        }

        [Fact]
        public async Task TestarConsultaTodos()
        {
            var model = new FilterProdutoInput
            {
                PorPagina = 10,
                Pagina = 1,
                CampoFiltro = "",
                ValorFiltro = ""
            };

            var response = await _produtoApi.RetornarVarios(model);

            response.StatusCode.Should().Be(HttpStatusCode.OK,
                $"* Ocorreu uma falha: Status Code esperado (200, OK) diferente do resultado gerado *");

            var resultado = response.Content;
        }

        [Fact]
        public async Task TestarExcluirPorId()
        {
            var modelInclusao = new ProdutoInput
            {
                Id = 0,
                Descricao = $"Produto Novo 01 {Guid.NewGuid()}",
                DataFabricacao = DateTime.Now,
                DataValidade = DateTime.Now.AddDays(1),
                FornecedorCodigo = "102030",
                FornecedorDescricao = "Fornecedor Produto",
                FornecedorCNPJ = "12345678901234",
                Situacao = 1
            };

            var responseInclusao = await _produtoApi.Incluir(modelInclusao);

            var modelCons = new FilterProdutoInput
            {
                PorPagina = 10,
                Pagina = 1,
                CampoFiltro = "",
                ValorFiltro = ""
            };

            var responseCons = await _produtoApi.RetornarVarios(modelCons);
            var registroExclusao = responseCons.Content.FirstOrDefault();            

            var model = new ParamIdInput
            {
                Id = registroExclusao.Id
            };

            var response = await _produtoApi.Excluir(model);

            response.StatusCode.Should().Be(HttpStatusCode.OK,
                $"* Ocorreu uma falha: Status Code esperado (200, OK) diferente do resultado gerado *");

            var resultado = response.Content;
        }

        [Theory]
        [ClassData(typeof(ProdClassData))]
        public async Task TestarInclusao(List<ProdutoInput> prods)
        {
            var model = new ProdutoInput
            {
                Id = 0,
                Descricao = $"Produto Novo 01 {Guid.NewGuid()}",
                DataFabricacao = DateTime.Now,
                DataValidade = DateTime.Now.AddDays(10),
                FornecedorCodigo = "102030",
                FornecedorDescricao = "Fornecedor Produto",
                FornecedorCNPJ = "12345678901234",
                Situacao = 1
            };

            var response = await _produtoApi.Incluir(prods.FirstOrDefault());
            response.StatusCode.Should().Be(HttpStatusCode.OK,
                $"* Ocorreu uma falha: Status Code esperado (200, OK) diferente do resultado gerado *");

            var resultado = response.Content;            
        }

        [Theory]
        [ClassData(typeof(ProdClassData))]
        public async Task TestarInclusaoFalhaData(List<ProdutoInput> prods)
        {
            var model = new ProdutoInput
            {
                Id = 0,
                Descricao = $"Produto Novo 01 {Guid.NewGuid()}",
                DataFabricacao = DateTime.Now,
                DataValidade = DateTime.Now.AddDays(-1),
                FornecedorCodigo = "102030",
                FornecedorDescricao = "Fornecedor Produto",
                FornecedorCNPJ = "12345678901234",
                Situacao = 1
            };

            var response = await _produtoApi.Incluir(model);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
                $"* Não retornou BadRequest. Não pode aceitar inclusão de Produto com data de fabricação maior ou igual a Validade *");

            var resultado = response.Content;
        }

        [Fact]        
        public async Task TestarEdicao()
        {
            var modelInclusao = new ProdutoInput
            {
                Id = 0,
                Descricao = $"Produto Novo 01 {Guid.NewGuid()}",
                DataFabricacao = DateTime.Now,
                DataValidade = DateTime.Now.AddDays(1),
                FornecedorCodigo = "102030",
                FornecedorDescricao = "Fornecedor Produto",
                FornecedorCNPJ = "12345678901234",
                Situacao = 1
            };

            var responseInclusao = await _produtoApi.Incluir(modelInclusao);

            var modelCons = new FilterProdutoInput
            {
                PorPagina = 10,
                Pagina = 1,
                CampoFiltro = "",
                ValorFiltro = ""
            };

            var responseCons = await _produtoApi.RetornarVarios(modelCons);
            var registroEdicao = responseCons.Content.FirstOrDefault();

            var modelEdicao = new ProdutoInput
            {
                Id = registroEdicao.Id,
                Descricao = $"Produto Edicao 01 {Guid.NewGuid()}",
                DataFabricacao = registroEdicao.DataFabricacao,
                DataValidade = registroEdicao.DataValidade,
                FornecedorCodigo = registroEdicao.FornecedorCodigo,
                FornecedorDescricao = registroEdicao.FornecedorDescricao,
                FornecedorCNPJ = registroEdicao.FornecedorCNPJ,
                Situacao = 1
            };

            var responseEdicao = await _produtoApi.Editar(modelEdicao);
            responseEdicao.StatusCode.Should().Be(HttpStatusCode.OK,
                $"* Ocorreu uma falha: Status Code esperado (200, OK) diferente do resultado gerado *");

            var resultado = responseEdicao.Content;
        }

        public class ProdClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {
                    new List<ProdutoInput>() {
                        new ProdutoInput
                        {
                            Id = 0,
                            Descricao = $"Produto Novo 01 {Guid.NewGuid()}",
                            DataFabricacao = DateTime.Now,
                            DataValidade = DateTime.Now.AddDays(10),
                            FornecedorCodigo = "102030",
                            FornecedorDescricao = "Fornecedor Produto",
                            FornecedorCNPJ = "12345678901234",
                            Situacao = 1
                        },
                        new ProdutoInput
                        {
                            Id = 0,
                            Descricao = $"Produto Novo 02 {Guid.NewGuid()}",
                            DataFabricacao = DateTime.Now,
                            DataValidade = DateTime.Now.AddDays(10),
                            FornecedorCodigo = "102030",
                            FornecedorDescricao = "Fornecedor Produto",
                            FornecedorCNPJ = "12345678901234",
                            Situacao = 0
                        }
                    }
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
