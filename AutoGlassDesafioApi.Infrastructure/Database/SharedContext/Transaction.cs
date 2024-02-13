using AutoGlassDesafioApi.Domain.SharedContext.Interfaces.Transaticon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Infrastructure.Database.SharedContext
{
    public class Transaction : ITransaction
    {
        private readonly ContextoPrincipal _contexto;

        public Transaction(ContextoPrincipal contexto)
        {
            _contexto = contexto;
        }

        public async Task BeginTransactionAsync()
        {
            await _contexto.Database.BeginTransactionAsync();
        }

        public void Commit()
        {
            _contexto.Database.CommitTransaction();
        }

        public void Rollback()
        {
            _contexto.Database.RollbackTransaction();
        }

        public async Task SaveChangesAsync()
        {
            await _contexto.SaveChangesAsync();
        }
    }
}
