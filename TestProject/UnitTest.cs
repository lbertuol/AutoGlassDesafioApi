using AutoGlassDesafioApi.Application.ProdutoContext.Interfaces;
using AutoGlassDesafioApi.Application.ProdutoContext.Models;
using AutoGlassDesafioApi.Application.ProdutoContext.Services;
using AutoGlassDesafioApi.Domain.ProdutosContext.DTO;
using AutoGlassDesafioApi.Domain.ProdutosContext.Enum;
using AutoGlassDesafioApi.Domain.ProdutosContext.Interfaces.EF;
using AutoGlassDesafioApi.Infrastructure.Database.ProdutosContext.Repository.Entity;
using AutoGlassDesafioApi.Infrastructure.Database.SharedContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class UnitTest
    {
        private readonly DbContextOptions<ContextoPrincipal> _dbContextOptions;
        private readonly IRepositoryProdutoWriteRead _produtoRepository;        
        private ContextoPrincipal dbContext;

        public UnitTest()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ContextoPrincipal>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Usar um nome de banco de dados exclusivo para cada teste
                .Options;

            dbContext = new ContextoPrincipal(_dbContextOptions);            
            _produtoRepository = new RepositoryProdutoWriteRead(dbContext);            
        }        

        [Fact]
        public async Task AddProd_ShouldSucceed()
        {
            // Arrange            
            var produto = Produto.Create(
                    id: 0,
                    descricao: $"Produto Novo 01 {Guid.NewGuid()}",
                    dataFabricacao: DateTime.Now,
                    dataValidade: DateTime.Now.AddDays(10),
                    fornecedorCodigo: "102030",
                    fornecedorDescricao: "Fornecedor Produto",
                    fornecedorCNPJ: "12345678901234");

            // Act
             _produtoRepository.Salvar(produto);
            dbContext.SaveChangesAsync().Wait();

            // Assert
            var retorno = await _produtoRepository.RetornarPorExpressionAsync(x => x.Descricao == produto.Descricao);

            Produto entidade = retorno.Match(
                some: retorno => retorno,
                none: () => {                    
                    return default(Produto);
                }
            );

            Assert.NotNull(entidade);
            Assert.NotEqual(0, entidade.Id);
        }

        [Fact]
        public async Task UpdateProd_ShouldSucceed()
        {            
            // Arrange            
            var produto = Produto.Create(
                    id: 0,
                    descricao: $"Produto Novo 02 {Guid.NewGuid()}",
                    dataFabricacao: DateTime.Now,
                    dataValidade: DateTime.Now.AddDays(10),
                    fornecedorCodigo: "102030",
                    fornecedorDescricao: "Fornecedor Produto",
                    fornecedorCNPJ: "12345678901234");
            
            _produtoRepository.Salvar(produto);
            dbContext.SaveChangesAsync().Wait();            

            var retorno = await _produtoRepository.RetornarPorExpressionAsync(x => x.Descricao == produto.Descricao);

            Produto entidade = retorno.Match(
                some: retorno => retorno,
                none: () => {
                    return default(Produto);
                }
            );            

            // Act
            var produtoUpdate = Produto.Create(
                    id: entidade.Id,
                    descricao: $"Produto updated 02 {Guid.NewGuid()}",
                    dataFabricacao: entidade.DataFabricacao,
                    dataValidade: entidade.DataValidade,
                    fornecedorCodigo: entidade.FornecedorCodigo,
                    fornecedorDescricao: entidade.FornecedorDescricao,
                    fornecedorCNPJ: entidade.FornecedorCNPJ);

            var local = dbContext.Set<Produto>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(entidade.Id));
            if (local != null)
            {
                // detach
                dbContext.Entry(local).State = EntityState.Detached;
            }
            // set Modified flag in your entry
            dbContext.Entry(produtoUpdate).State = EntityState.Modified;

            dbContext.SaveChangesAsync().Wait();


            _produtoRepository.Salvar(produtoUpdate);
            dbContext.SaveChangesAsync().Wait();            

            // Assert
            var retornoUpdated = await _produtoRepository.RetornarPorExpressionAsync(x => x.Descricao == produtoUpdate.Descricao);

            Produto entidadeUpdated = retornoUpdated.Match(
                some: retornoUpdated => retornoUpdated,
                none: () => {
                    return default(Produto);
                }
            );

            Assert.NotNull(entidadeUpdated);
            Assert.Equal(entidadeUpdated.Id, produtoUpdate.Id);            
        }

        [Fact]
        public async Task DeleteProd_ShouldSucceed()
        {            
            // Arrange            
            var produto = Produto.Create(
                    id: 0,
                    descricao: $"Produto Novo 03 {Guid.NewGuid()}",
                    dataFabricacao: DateTime.Now,
                    dataValidade: DateTime.Now.AddDays(10),
                    fornecedorCodigo: "102030",
                    fornecedorDescricao: "Fornecedor Produto",
                    fornecedorCNPJ: "12345678901234");

            _produtoRepository.Salvar(produto);
            dbContext.SaveChangesAsync().Wait();

            var retorno = await _produtoRepository.RetornarPorExpressionAsync(x => x.Descricao == produto.Descricao);

            Produto entidade = retorno.Match(
                some: retorno => retorno,
                none: () => {
                    return default(Produto);
                }
            );

            // Act
            _produtoRepository.Excluir(entidade);
            dbContext.SaveChangesAsync().Wait();

            // Assert
            var deletedUser = await _produtoRepository.RetornarPorIdAsync(entidade.Id);

            Produto entidadeDel = deletedUser.Match(
                some: deletedUser => deletedUser,
                none: () => {
                    return default(Produto);
                }
            );

            Assert.Null(entidadeDel);
        }

        [Fact]
        public async Task GetById_ShouldReturnProd()
        {
            // Arrange
            var produto = Produto.Create(
                    id: 0,
                    descricao: $"Produto Novo 04 {Guid.NewGuid()}",
                    dataFabricacao: DateTime.Now,
                    dataValidade: DateTime.Now.AddDays(10),
                    fornecedorCodigo: "102030",
                    fornecedorDescricao: "Fornecedor Produto",
                    fornecedorCNPJ: "12345678901234");

            _produtoRepository.Salvar(produto);
            dbContext.SaveChangesAsync().Wait();

            var retorno = await _produtoRepository.RetornarPorExpressionAsync(x => x.Descricao == produto.Descricao);

            Produto entidade = retorno.Match(
                some: retorno => retorno,
                none: () => {
                    return default(Produto);
                }
            );

            // Act
            var retornoPorId = await _produtoRepository.RetornarPorIdAsync(entidade.Id);

            Produto entidadePorId = retornoPorId.Match(
                some: retornoPorId => retornoPorId,
                none: () => {
                    return default(Produto);
                }
            );

            // Assert
            Assert.NotNull(entidadePorId);
            Assert.Equal(entidade.Id, entidadePorId.Id);
            Assert.Equal(entidade.Descricao, entidadePorId.Descricao);            
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllProds()
        {
            // Arrange
            var produto = Produto.Create(
                    id: 0,
                    descricao: $"Produto Novo 05 {Guid.NewGuid()}",
                    dataFabricacao: DateTime.Now,
                    dataValidade: DateTime.Now.AddDays(10),
                    fornecedorCodigo: "102030",
                    fornecedorDescricao: "Fornecedor Produto",
                    fornecedorCNPJ: "12345678901234");

            _produtoRepository.Salvar(produto);
            dbContext.SaveChangesAsync().Wait();            

            produto = Produto.Create(
                    id: 0,
                    descricao: $"Produto Novo 06 {Guid.NewGuid()}",
                    dataFabricacao: DateTime.Now,
                    dataValidade: DateTime.Now.AddDays(10),
                    fornecedorCodigo: "102030",
                    fornecedorDescricao: "Fornecedor Produto",
                    fornecedorCNPJ: "12345678901234");

            _produtoRepository.Salvar(produto);
            dbContext.SaveChangesAsync().Wait();

            // Act
            var produtosList = await _produtoRepository.RetornarVariosAsync();

            IEnumerable<Produto> entidadeList = produtosList.Match(
               some: produtosList => produtosList,
               none: () => {
                   return Enumerable.Empty<Produto>();
               }
           );

            // Assert
            Assert.NotNull(entidadeList);
            Assert.Equal(2, entidadeList.Count());
        }
    }
}