using AutoGlassDesafioApi.Application.ProdutoContext.Models;
using AutoGlassDesafioApi.Application.SharedContext.Mappers;
using AutoGlassDesafioApi.Domain.ProdutosContext.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Application.ProdutoContext.Mappers
{    
    public static class ProdutoMappingIn
    {
        public static void ProdutoMap(this InputModelToDomainMappingProfile profile)
        {
            profile.CreateMap<ProdutoInput, Produto>()
                .ForMember(x => x.Codigo, opt => opt.Ignore())
                .ForAllMembers(opts =>
                {
                    opts.Condition((src, dest, srcMember) => srcMember != null);
                });            
        }
    }
}
