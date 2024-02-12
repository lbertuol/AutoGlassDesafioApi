using AutoGlassDesafioApi.Domain.ProdutosContext.DTO;
using AutoGlassDesafioApi.Domain.SharedContext.Interfaces.EF;
using Optional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Domain.ProdutosContext.Interfaces.EF
{    
    public interface IRepositoryProdutoWriteRead : IRepository<Produto>
    {
        
    }
}
