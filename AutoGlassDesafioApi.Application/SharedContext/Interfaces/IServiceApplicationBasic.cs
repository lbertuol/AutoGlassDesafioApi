using AutoGlassDesafioApi.Application.SharedContext.Models;
using AutoGlassDesafioApi.Domain.SharedContext.DTO;
using AutoGlassDesafioApi.Domain.SharedContext.Filters;
using AutoGlassDesafioApi.Domain.SharedContext.Interfaces.Notifications;
using AutoGlassDesafioApi.Domain.SharedContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Application.SharedContext.Interfaces
{    
    public interface IServiceApplicationBasic<T, In, Out, InFiltro>
       where T : EntityBasic
       where In : InputBasic
       where Out : OutputBasic
       where InFiltro : FilterBasicInput
    {
        Task<Out> RetornarPorIdAsync(int id);
        Task<IEnumerable<Out>> RetornarVariosAsync();
        Task<IEnumerable<Out>> ListarAsync(InFiltro filtros);
        Task SalvarAsync(In input);
        Task ExcluirAsync(int id);
        INotification RetornarNotificacao();
    }
}
