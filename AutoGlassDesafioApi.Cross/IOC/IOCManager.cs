using AutoGlassDesafioApi.Application.ProdutoContext.Interfaces;
using AutoGlassDesafioApi.Application.ProdutoContext.Services;
using AutoGlassDesafioApi.Domain.ProdutosContext.Interfaces.Dapper;
using AutoGlassDesafioApi.Domain.ProdutosContext.Interfaces.EF;
using AutoGlassDesafioApi.Domain.ProdutosContext.Services;
using AutoGlassDesafioApi.Domain.SharedContext.Interfaces.Notifications;
using AutoGlassDesafioApi.Domain.SharedContext.Interfaces.Transaticon;
using AutoGlassDesafioApi.Domain.SharedContext.Services;
using AutoGlassDesafioApi.Domain.SharedContext.Validations;
using AutoGlassDesafioApi.Infrastructure.Database.ProdutosContext.Repository.Dapper;
using AutoGlassDesafioApi.Infrastructure.Database.ProdutosContext.Repository.Entity;
using AutoGlassDesafioApi.Infrastructure.Database.SharedContext;
using AutoGlassDesafioApi.Infrastructure.Database.SharedContext.Repository.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Cross.IOC
{
    public static class IOCManager
    {
        public static void Register(string connectionString, IServiceCollection services)
        {
            services.AddDbContext<ContextoPrincipal>();

            services.AddTransient<IStartupFilter, MigrationStartupFilter<ContextoPrincipal>>();
            services.AddScoped<ITransaction, Transaction>();
            services.AddScoped<INotification, Notification>();

            // REPOSITORIO ENTITYFRAMEWORK
            services.AddScoped<IRepositoryProdutoWriteRead, RepositoryProdutoWriteRead>();

            // REPOSITORIO DAPPER            
            services.AddScoped<IRepositoryProdutoRead, RepositoryProdutoRead>();

            // SERVICOS                       
            services.AddScoped<IServiceProduto, ServiceProduto>();

            //APLICACAO SERVICO
            services.AddScoped<IServiceApplicationProduto, ServiceApplicationProduto>();

            var coreAssembly = System.Reflection.Assembly.GetAssembly(typeof(ServiceProduto));
            var infraAssembly = System.Reflection.Assembly.GetAssembly(typeof(RepositoryProdutoWriteRead));
            // SERVICOS                       
            var classesServico = coreAssembly
            .GetTypes()
                    .Where(x => !x.IsAbstract && x.IsSubclassOf(typeof(ServiceBasic<>)))
                    .ToArray();
            foreach (var c in classesServico)
                services.AddScoped(c);
        }
    }
}
