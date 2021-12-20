using netbullAPI.Entidade;
using netbullAPI.Interfaces;
using netbullAPI.Util;

namespace netbullAPI.Persistencia
{
    public class DAOItem : DaoBase
    {
        private netbullDBContext netbullDBContext;
        public DAOItem(INotificador notificador, IConfiguration configuration, netbullDBContext netbullDBContext) : base(notificador, configuration)
        {
            this.netbullDBContext = netbullDBContext;
        }
        public IEnumerable<Item> BuscaItemPedido(int id)
        {
            try
            {
                var pedido = netbullDBContext.Pedidos.Where(p => p.pedido_id == id).FirstOrDefault();
                if (pedido == null)
                {
                    Notificar("Pedido informado inexistente");
                    return null;
                }
                else
                {
                    var itens = from item in netbullDBContext.Itens
                                where item.item_idPedido == id
                                select item;
                    return itens;
                }
            }
            catch (Exception e)
            {
                Notificar(e.Message);
                throw e;
            }
        }
        public Item AdicionaItem(Item item)
        {
            var pedido = netbullDBContext.Pedidos.Where(p => p.pedido_id == item.item_idPedido).FirstOrDefault();
            if (pedido == null)
            {
                Notificar("Pedido informado inexistente");
                return null;
            }
            try
            {
                var produto = netbullDBContext.Produtos.Where(p => p.produto_id == item.item_idProduto).FirstOrDefault();
                Item novo_item = new Item()
                {
                    item_id = netbullDBContext.Itens.Max(m => m.item_id) + 1,
                    item_idPedido = item.item_idPedido,
                    item_qtdproduto = item.item_qtdproduto,
                    item_idProduto = produto.produto_id,
                    item_valor = item.CalculaValorItem(item.item_qtdproduto, produto.produto_valor)
                };
                netbullDBContext.Add(novo_item);
                netbullDBContext.SaveChanges();
                return novo_item;
            }
            catch(Exception e)
            {
                Notificar(e.Message);
                throw e;
            }
        }
        public bool DeletaItem(int id)
        {
            var item_existente = netbullDBContext.Itens.Where(item => item.item_id == id).FirstOrDefault();
            if(item_existente == null)
            {
                Notificar("Item informado inexistente");
                return false;
            }
            netbullDBContext.Remove(item_existente);
            netbullDBContext.SaveChanges();
            return true;
        }
        public bool AlteraQuantidadeProduto(int id, int quantidade)
        {
            var item_existente = netbullDBContext.Itens.Where(i => i.item_id == id).FirstOrDefault();
            if (item_existente == null)
            {
                Notificar("Item informado inexistente");
                return false;
            }
            else
            {
                var produto = netbullDBContext.Produtos.Where(p => p.produto_id == item_existente.item_idProduto).FirstOrDefault();
                item_existente.item_qtdproduto = quantidade;
                item_existente.item_valor = item_existente.CalculaValorItem(quantidade, produto.produto_valor);
                netbullDBContext.Update(item_existente);
                netbullDBContext.SaveChanges();
                return true;
            }
        }
    }
}
