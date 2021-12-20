using netbullAPI.Entidade;
using netbullAPI.Persistencia;
using netbullAPI.Repository;
using netbullAPI.ViewModels;

namespace netbullAPI.MidwareDB
{
    public class NE_Produto
    {
        private DAOProduto _daoProduto;

        public NE_Produto(DAOProduto daoProduto)
        {
            _daoProduto = daoProduto;
        }

        public async Task<List<Produto>> GetAllAsync()
        {
            return await _daoProduto.GetAllAsync();
        }

        public async Task<Produto> GetPorIdAsync(int id)
        {
            return await _daoProduto.GetPorIdAsync(id);
        }

        public async Task<Produto> AdicionaProduto(RegistrarProdutoViewModel produto)
        {
            return await _daoProduto.AdicionaProdutoAsync(produto);
        }

        public async Task<Produto> AtualizaProduto(Produto produto)
        {
            try
            {
                return await _daoProduto.AtualizaProdutoAsync(produto);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        public async Task<Produto> AtualizaProdutoPatch(CampoEditar campoEditar, Produto produto)
        {
            try
            {
                return await _daoProduto.AtualizaProdutoCampoAsync(campoEditar,produto);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool DeletaProduto(int id)
        {
            try
            {
                return _daoProduto.DeletaProduto(id);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
   
}
