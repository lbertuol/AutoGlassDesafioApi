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
            var model = new ParamIdInput
            {
                Id = 5
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
                Descricao = "",
                DataFabricacao = DateTime.Now,
                DataValidade = DateTime.Now,
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

        [Fact]        
        public async Task TestarEdicao()
        {
            var model = new ProdutoInput
            {
                Id = 1,
                Descricao = "",
                DataFabricacao = DateTime.Now,
                DataValidade = DateTime.Now,
                FornecedorCodigo = "102030",
                FornecedorDescricao = "Fornecedor Produto",
                FornecedorCNPJ = "12345678901234",
                Situacao = 1
            };

            var response = await _produtoApi.Editar(model);
            response.StatusCode.Should().Be(HttpStatusCode.OK,
                $"* Ocorreu uma falha: Status Code esperado (200, OK) diferente do resultado gerado *");

            var resultado = response.Content;
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
