using netbullAPI.Interfaces;
using netbullAPI.Util;

namespace netbullAPI.MidwareDB
{
    public class Notificador : INotificador
    {
        public List<Notificacao> notificacoes;

        public Notificador()
        {
            this.notificacoes = new List<Notificacao>();
        }

        public void Adicionar(Notificacao notificacao)
        {
            this.notificacoes.Add(notificacao);
        }

        public List<Notificacao> ObterNotificacoes()
        {
            return this.notificacoes;
        }
    }
}