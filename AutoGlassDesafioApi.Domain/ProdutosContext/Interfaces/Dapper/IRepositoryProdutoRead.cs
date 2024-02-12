using AutoGlassDesafioApi.Domain.ProdutosContext.DTO;
using AutoGlassDesafioApi.Domain.ProdutosContext.Filtros;
using AutoGlassDesafioApi.Domain.SharedContext.Interfaces.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Domain.ProdutosContext.Interfaces.Dapper
{   
    public interface IRepositoryProdutoRead : IRepositoryBaseRead<Produto, FilterProdutoInput>
    {

    }
}
