using netbullAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace netbullAPI.Util
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly INotificador _notificador;

        public BaseController(INotificador notificador)
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
