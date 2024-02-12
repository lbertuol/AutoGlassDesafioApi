using Optional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Domain.SharedContext.Interfaces.Dapper
{   
    public interface IRepositoryBaseRead<T, In>
        where T : class
        where In : class
    {
        Task<Option<IEnumerable<T>>> ListarAsync(In filtros);
        Task<Option<IEnumerable<T>>> RetornarTodosAsync(string instrucaoSQL);
        Task<int> RetornarInteiroAsync(string instrucaoSQL);
    }
}
