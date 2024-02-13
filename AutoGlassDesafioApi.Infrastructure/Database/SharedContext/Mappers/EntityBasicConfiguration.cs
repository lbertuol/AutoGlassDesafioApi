using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoGlassDesafioApi.Domain.SharedContext.DTO;

namespace AutoGlassDesafioApi.Infrastructure.Database.SharedContext.Mappers
{    
    public abstract class EntityBasicConfiguration<T> : IEntityTypeConfiguration<T> where T : EntityBasic
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
