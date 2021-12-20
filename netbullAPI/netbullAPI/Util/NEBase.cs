using netbullAPI.Interfaces;
using netbullAPI.Util;
using Npgsql;

namespace netbullAPI.Util
{
    public class NEBase
    {
        private readonly INotificador _notificador;

        public NEBase(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected void Notificar(string mensagem)
        {
            this._notificador.Adicionar(new Notificacao(mensagem));
        }

        protected List<Notificacao> Notificacoes()
        {
            return this._notificador.ObterNotificacoes();
        }
    }
       
}
