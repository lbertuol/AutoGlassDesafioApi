using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Domain.SharedContext.Interfaces.Transaticon
{    
    public interface ITransaction
    {
        Task BeginTransactionAsync();
        void Commit();
        void Rollback();
        Task SaveChangesAsync();
    }
}
