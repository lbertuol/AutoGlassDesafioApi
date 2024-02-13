using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Application.SharedContext.Mappers
{
    public class AutoMapperConfig
    {
        public static IMapper Mapper { get; set; }

        public static MapperConfiguration RegisterMappings(IServiceProvider serviceProvider)
        {
            var config = new MapperConfiguration(cfg => {

                cfg.AddProfile(new InputModelToDomainMappingProfile());
                cfg.AddProfile(new DomainToInputModelMappingProfile());                
            });

            config.AssertConfigurationIsValid();
            return config;
        }
    }
}
