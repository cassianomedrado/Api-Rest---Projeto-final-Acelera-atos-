using netbullAPI.Entidade;
using netbullAPI.Interfaces;
using netbullAPI.Util;

namespace netbullAPI.Persistencia
{
    public class DAO_Pessoa : DaoBase
    {
        private netbullDBContext _PessoaContexto { get; set; }
        public DAO_Pessoa(INotificador notificador, IConfiguration configuration, netbullDBContext context) : base(notificador, configuration)
        {
            _PessoaContexto = context;
        }

        public async Task<IEnumerable<Pessoa>> BuscaPessoas()
        {
            try
            {
                var pessoas =  _PessoaContexto.Pessoas.AsEnumerable();
                if (pessoas == null)
                {
                    Notificar("Não existem cadastros");
                    return pessoas.OrderBy(p => p.pessoa_id);
                }
                else
                {
                    return pessoas.OrderBy(p => p.pessoa_id); ;
                }               
            }
            catch (Exception e)
            {
                Notificar(e.Message);
                throw;
            }
        }

        public async Task<Pessoa> BuscaPessoaPorId(int id)
        {
            try
            {
                var pessoa = _PessoaContexto.Pessoas.Where(p => p.pessoa_id == id).FirstOrDefault();
                if (pessoa == null)
                {
                    Notificar("Id incorreto ou pessoa inexistente");
                    return pessoa;
                }
                else
                {
                    return pessoa;
                }                
            }
            catch (Exception e)
            {
                Notificar("Erro Interno");
                throw;
            }

        }
        
        public async Task<bool> InserirPessoa(Pessoa pessoa)
        {
            Pessoa pessoaRetorno = null;
            try
            {
                var addPessoa = _PessoaContexto.Pessoas.Any(p => p.pessoa_id == pessoa.pessoa_id);
                if (addPessoa == true)
                {
                    Notificar("Id já atribuido ou pessoa já está cadastrada");
                    return false;
                }
                else
                {
                    _PessoaContexto.Pessoas.Add(pessoa);
                    _PessoaContexto.SaveChanges();

                    pessoaRetorno = _PessoaContexto.Pessoas.Where(p => p.pessoa_documento.Equals(pessoa.pessoa_documento) &&
                                                                       p.pessoa_nome.Equals(pessoa.pessoa_nome) &&
                                                                       p.pessoa_tipopessoa == pessoa.pessoa_tipopessoa ).FirstOrDefault();
                    pessoa = pessoaRetorno;
                    return true;
                }                
            }
            catch (Exception e)
            {
                Notificar("Erro Interno");
                return false;
            }

        }
        public async Task<Pessoa> UpdatePessoa(Pessoa pessoa)
        {
            try
            {
                var pessoaSelecionada = _PessoaContexto.Pessoas.Where(p => p.pessoa_id == pessoa.pessoa_id).FirstOrDefault();
                if (pessoaSelecionada == null)
                {
                    Notificar("Pessoa não encontrada");
                    return pessoaSelecionada;
                }
                else
                {
                    pessoaSelecionada.pessoa_nome = pessoa.pessoa_nome;
                    pessoaSelecionada.pessoa_tipopessoa = pessoa.pessoa_tipopessoa;
                    pessoaSelecionada.pessoa_documento = pessoa.pessoa_documento;
                    _PessoaContexto.SaveChanges();
                    return pessoaSelecionada;
                }
                
            }
            catch (Exception e)
            {
                Notificar("Erro Interno");
                throw;
            }

        }

        public async Task<bool> DeletarPessoa(int id)
        {
            try
            {
                var pessoaSelecionada = _PessoaContexto.Pessoas.Where(p => p.pessoa_id == id).FirstOrDefault();
                if (pessoaSelecionada == null)
                {
                    Notificar("Pessoa informada inexistente");
                    return false;
                }
                else
                {
                    _PessoaContexto.Pessoas.Remove(pessoaSelecionada);
                    _PessoaContexto.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Notificar("Erro Interno");
                throw;
            }

        }
    }
}
