using netbullAPI.Entidade;
using netbullAPI.Persistencia;
using netbullAPI.ViewModels;

namespace netbullAPI.MidwareDB
{
    public class NE_Pedido
    {
        private DAOPedido daoPedido;
        public NE_Pedido(DAOPedido daoPedido)
        {
            this.daoPedido = daoPedido;
        }
        public IEnumerable<RetornaPedidoViewModel> BuscaPedidosCliente(int id)
        {
            return daoPedido.BuscaPedidosPessoa(id);
        }

        public List<Pedido> BuscaPedidosUsuario(string nome)
        {
            return daoPedido.BuscaPedidosUser(nome);
        }
        public Pedido AdicionaPedido(Pedido pedido)
        {
            return daoPedido.AdicionaPedido(pedido);
        }
        public Pedido AlteraStatusPedido(int id, EnumStatusPedido status)
        {
            return daoPedido.AlteraStatusPedido(id, status);
        }

        public bool DeletaPedido(int id)
        {
            return daoPedido.DeletaPedido(id);
        }
    }
}
