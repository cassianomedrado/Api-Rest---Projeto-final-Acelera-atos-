using netbullAPI.Entidade;
using netbullAPI.Persistencia;
using netbullAPI.ViewModels;

namespace netbullAPI.MidwareDB
{
    public class NE_Endereco
    {
        private DAO_Endereco _daoEndereco;
        public NE_Endereco(DAO_Endereco daoEndereco)
        {
            _daoEndereco = daoEndereco;
        }

        public async Task<IEnumerable<Endereco>> BuscaEnderecosPessoaAsync(int idPessoa)
        {
            return await _daoEndereco.BuscaEnderecosPessoaAsync(idPessoa);
        }

        public async Task<bool> CadastraNovoEnderecoAsync(RegistrarEnderecoViewModel endereco)
        {
            return await _daoEndereco.CadastrarNovoEnderecoAsync(endereco);
        }
        public async Task<bool> AtualizaEnderecoAsync(AlterarEnderecoViewModel endereco, int idEndereco)
        {
            return await _daoEndereco.AtualizaEnderecoAsync(endereco, idEndereco);
        }

        public async Task<bool> AtualizaEnderecoLogradouroPatchAsync(int idEndereco, string logradouro)
        {
            return await _daoEndereco.AtualizaEnderecoLogradouroPatchAsync(idEndereco, logradouro);
        }

        public async Task<bool> ApagaEnderecoAsync(int idEndereco)
        {
            return await _daoEndereco.ApagaEnderecoAsync(idEndereco);
        }
    }
}