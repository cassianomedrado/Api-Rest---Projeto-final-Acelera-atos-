using netbullAPI.Entidade;
using netbullAPI.Persistencia;

namespace netbullAPI.MidwareDB
{
    public class NE_Pessoa
    {
        private DAO_Pessoa _daoPessoa;

        public NE_Pessoa(DAO_Pessoa daoPessoa)
        {
            _daoPessoa = daoPessoa;
        }

        public async Task<IEnumerable<Pessoa>> BuscaPessoas()
        {
            var pessoas = await _daoPessoa.BuscaPessoas();
            return pessoas;
        }

        public async Task<Pessoa> BuscaPessoaPorId(int id)
        {
          var pessoa = await _daoPessoa.BuscaPessoaPorId(id);
            return pessoa;
        }
        
        public async Task<bool> DeletarPessoa(int id)
        {
            var delPessoa = await _daoPessoa.DeletarPessoa(id);
            return delPessoa;
        }
        public async Task<bool> InserirPessoa(Pessoa pessoa)
        {
            var respostaInsercao = await _daoPessoa.InserirPessoa(pessoa);
            return respostaInsercao;
        }
        public async Task<Pessoa> AtualizarPessoa(Pessoa pessoa)
        {
            var attPessoa = await _daoPessoa.UpdatePessoa(pessoa);
            return attPessoa;
        }
    }
}