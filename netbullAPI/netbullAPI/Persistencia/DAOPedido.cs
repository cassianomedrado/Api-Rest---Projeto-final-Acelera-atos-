using Dapper;
using netbullAPI.Entidade;
using netbullAPI.Interfaces;
using netbullAPI.Security.Models;
using netbullAPI.Util;
using netbullAPI.ViewModels;

namespace netbullAPI.Persistencia
{
    public class DAOPedido : DaoBase
    {
        private netbullDBContext netbullDBContext;
        private IConfiguration configuration;
        public DAOPedido(INotificador notificador, IConfiguration configuration, netbullDBContext netbullDBContext) : base(notificador, configuration)
        {
            this.netbullDBContext = netbullDBContext;
            this.configuration = configuration;
        }

        public IEnumerable<RetornaPedidoViewModel> BuscaPedidosPessoa(int id)
        {
            try
            {
                var pessoa = netbullDBContext.Pessoas.Where(pessoa => pessoa.pessoa_id == id).FirstOrDefault();
                if (pessoa == null)
                {
                    Notificar("Cliente informado inexistente");
                    return null;
                }
                else
                {
                    var historico_pedidos = from pedido in netbullDBContext.Pedidos
                                            where pedido.pedido_idPessoa == id
                                            select new RetornaPedidoViewModel()
                                            {
                                                pedido = pedido,
                                                itens = netbullDBContext.Itens.Where(i => i.item_idPedido == pedido.pedido_id).ToList(),
                                            };
                    return historico_pedidos;
                }
            }
            catch (Exception e)
            {
                Notificar(e.Message);
                throw;
            }
            
        }

        public IEnumerable<RetornaPedidoViewModel> BuscaPedidosUsuario(int id)
        {
            try
            {
                var user = netbullDBContext.Users.Where(u => u.user_id == id).FirstOrDefault();
                if (user == null)
                {
                    Notificar("Usuário informado inexistente");
                    return null;
                }
                else
                {
                    var historico_pedidos = from pedido in netbullDBContext.Pedidos
                                            where pedido.pedido_idUsuario == id
                                            select new RetornaPedidoViewModel()
                                            {
                                                pedido = pedido,
                                                itens = netbullDBContext.Itens.Where(i => i.item_idPedido == pedido.pedido_id).ToList(),
                                            };
                    return historico_pedidos;
                }
            }
            catch (Exception e)
            {
                Notificar(e.Message);
                throw;
            }

        }

        public List<Pedido> BuscaPedidosUser(string name)
        {
            try
            {
                User user;

                string sqlUser = $@"SELECT * FROM users WHERE user_nome = '{name}'";
                var connection = getConnection();

                using (connection)
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {

                        user = connection.Query<User>(sqlUser, transaction).FirstOrDefault();

                        transaction.Commit();
                    }
                }

                if (user == null)
                {
                    Notificar("Usuário informado inexistente");
                    return null;
                }

                var pedidos = from pedido in netbullDBContext.Pedidos
                                        where pedido.pedido_idUsuario == user.user_id
                                        select pedido;
                return pedidos.ToList();
            }
            catch (Exception e)
            {
                Notificar(e.Message);
                throw;
            }
            
        }

        public bool DeletaPedido(int id)
        {
            var pedido_existente = netbullDBContext.Pedidos.Where(pedido => pedido.pedido_id == id).FirstOrDefault();
            if (pedido_existente == null)
            {
                Notificar("Pedido informado inexistente");
                return false;
            }
            netbullDBContext.Remove(pedido_existente);
            netbullDBContext.SaveChanges();
            return true;
        }

        public Pedido AlteraStatusPedido(int id, EnumStatusPedido status)
        {
            var pedido_existente = netbullDBContext.Pedidos.Where(p => p.pedido_id == id).FirstOrDefault();
            if(pedido_existente == null)
            {
                Notificar("Pedido informado inexistente");
                return null;
            }
            else
            {
                pedido_existente.pedido_status = status;
                netbullDBContext.Update(pedido_existente);
                netbullDBContext.SaveChanges();
                return pedido_existente;
            }
        }

        public Pedido AdicionaPedido(Pedido pedido)
        {
            var pessoa = netbullDBContext.Pessoas.Where(p => p.pessoa_id == pedido.pedido_idPessoa).FirstOrDefault();
            if(pessoa == null)
            {
                Notificar("Cliente informado inexistente");
                return null;
            }
            try
            {
                Pedido novo_pedido = new Pedido()
                {
                    pedido_id = netbullDBContext.Pedidos.Max(m => m.pedido_id) + 1,
                    pedido_idPessoa = pedido.pedido_idPessoa,
                    pedido_status = EnumStatusPedido.pedido_reservado,
                    pedido_valor = pedido.pedido_valor,
                    pedido_time = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm"),
                    pedido_idEndereco = pedido.pedido_idEndereco,
                    pedido_idUsuario = pedido.pedido_idUsuario,
                };
                netbullDBContext.Add(novo_pedido);
                netbullDBContext.SaveChanges();
                return novo_pedido;
            }
            catch (Exception e)
            {
                Notificar(e.Message);
                throw e;
            }
        }
    }
}
