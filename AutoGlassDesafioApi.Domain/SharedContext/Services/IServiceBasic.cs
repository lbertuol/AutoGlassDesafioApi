using AutoGlassDesafioApi.Domain.SharedContext.DTO;
using AutoGlassDesafioApi.Domain.SharedContext.Interfaces.Notifications;
using Optional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Domain.SharedContext.Services
{
    public interface IServiceBasic<T> where T : EntityBasic
    {
        Task<Option<T>> RetornarPorIdAsync(int id);
        Task<Option<IQueryable<T>>> RetornarVariosAsync();
        Task<Option<T>> RetornarPorExpressionAsync(Expression<Func<T, bool>> predicate);
        void Salvar(T entidade);
        void Excluir(T entidade);
        INotification RetornarNotificacao();
    }
}
