using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoGlassDesafioApi.Domain.ProdutosContext.DTO;
using AutoGlassDesafioApi.Infrastructure.Database.SharedContext.Mappers;
using System.Reflection.Emit;

namespace AutoGlassDesafioApi.Infrastructure.Database.ProdutosContext.Mappers
{   
    public sealed class ProdutoConfiguration : EntityBasicConfiguration<Produto>
    {
        public override void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("produtos");
            base.Configure(builder);            
            builder.Property(x => x.Descricao).IsRequired().HasMaxLength(200);
            builder.Property(x => x.FornecedorDescricao).HasMaxLength(200);
            builder.Property(x => x.FornecedorCodigo).HasMaxLength(20);
            builder.Property(x => x.FornecedorCNPJ).HasMaxLength(14);            

            builder.Property(c => c.Codigo)
            .HasDefaultValueSql("NEXT VALUE FOR SequencialProduto");
        }
    }
}
