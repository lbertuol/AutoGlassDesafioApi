using AutoGlassDesafioApi.Domain.SharedContext.DTO;
using AutoGlassDesafioApi.Domain.SharedContext.Interfaces.EF;
using Microsoft.EntityFrameworkCore;
using Optional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Infrastructure.Database.SharedContext.Repository.Entity
{
    public abstract class RepositoryBaseEF<T> : IRepository<T> where T : EntityBasic
    {
        protected readonly ContextoPrincipal _contexto;
        protected readonly DbSet<T> dbSet;

        public RepositoryBaseEF(ContextoPrincipal contextoPrincipal)
        {
            _contexto = contextoPrincipal;
            dbSet = contextoPrincipal.Set<T>();
        }

        public virtual async Task<Option<T>> RetornarPorIdAsync(int id)
        {
            var consulta = await dbSet.FirstOrDefaultAsync(x => x.Id == id);
            return consulta == null ? Option.None<T>() : Option.Some<T>(consulta);
        }

        public virtual async Task<Option<T>> RetornarPorExpressionAsync(Expression<Func<T, bool>> predicate)
        {
            var consulta = await dbSet.FirstOrDefaultAsync(predicate);
            return consulta == null ? Option.None<T>() : Option.Some<T>(consulta);
        }

        public virtual async Task<Option<IQueryable<T>>> RetornarVariosAsync(Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> query;
            if (predicate != null)
                query = dbSet.Where(predicate);
            else
                query = dbSet;

            var list = await query.ToListAsync();
            var consulta = list.AsQueryable();
            return consulta == null ? Option.None<IQueryable<T>>() : Option.Some<IQueryable<T>>(consulta);
        }

        public virtual void Salvar(T entidade)
        {
            if (entidade.Id == 0)
            {
                _contexto.Add(entidade);
            }
            else
            {
                _contexto.Attach(entidade);
                _contexto.Entry(entidade).State = EntityState.Modified;
            }
        }

        public virtual void Excluir(T entidade)
        {
            _contexto.Remove(entidade);
        }

        #region Wrapper

        protected async Task<Option<IQueryable<T>>> RetornarColecaoWrapper(Func<Task<IQueryable<T>>> acao)
        {
            var retorno = await acao.Invoke();
            return retorno == null ? Option.None<IQueryable<T>>() : Option.Some<IQueryable<T>>(retorno);
        }

        protected async Task<Option<T>> RetornarUnicoWrapper(Func<Task<T>> acao, int codigo)
        {
            var retorno = await acao.Invoke();
            return retorno == null ? Option.None<T>() : Option.Some<T>(retorno);
        }

        #endregion
    }
}
