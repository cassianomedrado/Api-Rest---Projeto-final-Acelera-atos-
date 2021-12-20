using netbullAPI.Entidade;
using netbullAPI.Interfaces;
using netbullAPI.Persistencia;
using netbullAPI.Util;
using netbullAPI.ViewModels;

namespace netbullAPI.Repository
{

    public class DAOProduto : DaoBase
    {
        private netbullDBContext _netbullDBContext;
        public DAOProduto(INotificador notificador, IConfiguration configuration, netbullDBContext netbullDBContext) : base(notificador, configuration)
        {
            _netbullDBContext = netbullDBContext;
        }

        public async Task<List<Produto>> GetAllAsync()
        {
            List<Produto> context = null;
            try
            {                
                context = _netbullDBContext.Produtos.Where(x => x.produto_id != null).ToList();
                if (context == null)
                {
                    Notificar("Produtos não encontrados");
                }

                return context;

            }
            catch (Exception e)
            {
                Notificar("Não foi possível encontrar produtos.");
                return context;
            }
        }

        public async Task<Produto> GetPorIdAsync(int id)
        {
            try
            {
                var produtoExistente = _netbullDBContext.Produtos.Where(x => x.produto_id == id).FirstOrDefault();
                if (produtoExistente == null)
                {
                    Notificar("Produto não existente");
                }
                return produtoExistente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Produto> AdicionaProdutoAsync(RegistrarProdutoViewModel produto)
        {
            try
            {
                if (ValidaProduto(produto))
                {
                    var novoProduto = new Produto()
                    {
                        //Criado id dessa forma pois não tem autoincremento implementado
                        produto_id = _netbullDBContext.Produtos.Max(m => m.produto_id) + 1,
                        produto_nome = produto.produto_nome,
                        produto_valor = produto.produto_valor
                    };

                    _netbullDBContext.Add(novoProduto);
                    _netbullDBContext.SaveChanges();
                    return novoProduto;
                }
                else
                {
                    Notificar("Formatação inválida");
                    return null;
                }
            }
            catch (Exception e)
            {
                Notificar("Não foi possível incluir produto.");
                return null;
            }
        }

        private bool ValidaProduto(RegistrarProdutoViewModel produto)
        {
            if(string.IsNullOrEmpty(produto.produto_nome))
                return false;
            return true;
        }

        public async Task<Produto> AtualizaProdutoAsync(Produto produto)
        {
            try
            {
                var produtoExistente = _netbullDBContext.Produtos.Where(x => x.produto_id == produto.produto_id).FirstOrDefault();
                if(produtoExistente == null)
                {
                    Notificar("Produto não encontrado.");
                    return null;
                }
                if (!string.IsNullOrEmpty(produto.produto_nome))
                {
                    produtoExistente.produto_nome = produto.produto_nome;
                    produtoExistente.produto_valor = produto.produto_valor;
                    _netbullDBContext.Update(produtoExistente);
                    _netbullDBContext.SaveChanges();
                    return produtoExistente;
                }
                else
                {
                    Notificar("Produto com campos faltantes.");
                    return null;
                }
            }
            catch (Exception e)
            {
                Notificar("Não foi possível atualizar produto.");
                return null;
            }
        }

        public async Task<Produto> AtualizaProdutoCampoAsync(CampoEditar campo,Produto produto)
        {
            try
            {
                var produtoExistente = _netbullDBContext.Produtos.Where(x => x.produto_id == produto.produto_id).FirstOrDefault();
                if (produtoExistente == null)
                {
                    Notificar("Produto não encontrado.");
                    return null;
                }
                
                switch (campo)
                {
                    case CampoEditar.Nome:
                        if (string.IsNullOrEmpty(produto.produto_nome))
                        {
                            Notificar("Produto com campos faltantes.");
                            return null;
                        }
                        produtoExistente.produto_nome = produto.produto_nome;
                        break;
                    case CampoEditar.Valor:
                        produtoExistente.produto_valor = produto.produto_valor;
                        break;
                }
                _netbullDBContext.Update(produtoExistente);
                _netbullDBContext.SaveChanges();
                return produtoExistente;
                
            }
            catch (Exception e)
            {
                Notificar("Não foi possível atualizar produto.");
                return null;
            }
        }

        public bool DeletaProduto(int id)
        {
            try
            {
                var produtoExistente = _netbullDBContext.Produtos.Where(t => t.produto_id == id).FirstOrDefault();
                if (produtoExistente == null)
                {
                    Notificar("Produto informado inexistente");
                    return false;
                }

                _netbullDBContext.Remove(produtoExistente);
                _netbullDBContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Notificar("Não foi possível excluir o produto.");
                throw e;
            }
        }
    }

    public enum CampoEditar
    {
        Nome,
        Valor
    }
}