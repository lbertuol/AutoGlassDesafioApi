using AutoGlassDesafioApi.Domain.SharedContext.Interfaces.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGlassDesafioApi.Domain.SharedContext.Validations
{
    public class Notification: INotification
    {
        private List<string> _mensagem;

        public Notification()
        {
            _mensagem = new List<string>();
        }

        public void Adicionar(string mensagem)
        {
            _mensagem.Add(mensagem);
        }

        public bool IsValid()
        {
            return (_mensagem.Count == 0);
        }

        public List<string> RetornarErros()
        {
            return _mensagem;
        }
    }
}
