using AutoGlassDesafioApi.Application.SharedContext.Mappers;
using AutoGlassDesafioApi.Domain.ProdutosContext.DTO;
using AutoGlassDesafioApi.Domain.ProdutosContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Application.ProdutoContext.Mappers
{
    public static class ProdutoMappingOut
    {
        public static void ProdutoMap(this DomainToInputModelMappingProfile profile)
        {
            profile.CreateMap<Produto, ProdutoOutput>();
        }
    }
}
