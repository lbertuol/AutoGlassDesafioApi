using AutoGlassDesafioApi.Domain.ProdutosContext.DTO;
using AutoGlassDesafioApi.Infrastructure.Database.ProdutosContext.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static Dapper.SqlMapper;

namespace AutoGlassDesafioApi.Infrastructure.Database.SharedContext
{
    public class ContextoPrincipal : DbContext
    {
        private readonly IConfiguration _configuration;
        public ContextoPrincipal()
        {
        }

        public ContextoPrincipal(DbContextOptions<ContextoPrincipal> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conexao = ConexaoBanco();
            
            optionsBuilder
                .UseSqlServer(conexao,
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null);
                })
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProdutoConfiguration());

            modelBuilder.HasSequence<int>("SequencialProduto").StartsAt(1).IncrementsBy(1);

            base.OnModelCreating(modelBuilder);
        }

        private string ConexaoBanco()
        {
            return _configuration.GetSection("AppSettings").GetSection("ConnectionString").Value;
        }
    }
}
