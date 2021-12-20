using netbullAPI.Interfaces;
using netbullAPI.Util;
using Npgsql;

namespace netbullAPI.Util
{
    public class DaoBase
    {
        private readonly INotificador _notificador;
        private readonly IConfiguration _configuration;

        public DaoBase(INotificador notificador, IConfiguration configuration)
        {
            _notificador = notificador;
            _configuration = configuration; 
        }

        public NpgsqlConnection getConnection()
        {
            var connectionString = _configuration.GetConnectionString("NetBullConnection");

            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            return connection;
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
