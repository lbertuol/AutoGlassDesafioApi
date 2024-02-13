﻿using AutoGlassDesafioApi.Application.ProdutoContext.Mappers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Application.SharedContext.Mappers
{
    public sealed class InputModelToDomainMappingProfile : Profile
    {
        public InputModelToDomainMappingProfile()
        {
            this.ProdutoMap();           
        }
    }
}
