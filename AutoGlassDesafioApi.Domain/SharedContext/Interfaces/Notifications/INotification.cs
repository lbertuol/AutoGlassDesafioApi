using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Domain.SharedContext.Interfaces.Notifications
{
    public interface INotification
    {
        void Adicionar(string mensagem);
        bool IsValid();
        List<string> RetornarErros();
    }
}
