using netbullAPI.Entidade;

namespace netbullAPI.ViewModels
{
    public class RetornaPedidoViewModel
    {
        public Pedido pedido { get; set; }
        public List<Item> itens { get; set; }

    }
}
