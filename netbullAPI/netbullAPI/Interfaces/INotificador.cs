using netbullAPI.Util;

namespace netbullAPI.Interfaces
{
    public interface INotificador
    {
        public void Adicionar(Notificacao notificacao);
        public List<Notificacao> ObterNotificacoes();
    }
}
