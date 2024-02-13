using AutoGlassDesafioApi.Domain.ProdutosContext.DTO;
using AutoGlassDesafioApi.Domain.ProdutosContext.Interfaces.EF;
using AutoGlassDesafioApi.Infrastructure.Database.SharedContext;
using AutoGlassDesafioApi.Infrastructure.Database.SharedContext.Repository.Entity;
using Microsoft.EntityFrameworkCore;
using Optional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Infrastructure.Database.ProdutosContext.Repository.Entity
{    
    public class RepositoryProdutoWriteRead : RepositoryBaseEF<Produto>, IRepositoryProdutoWriteRead
    {
        public RepositoryProdutoWriteRead(ContextoPrincipal contextoPrincipal) : base(contextoPrincipal) { }        
    }
}
