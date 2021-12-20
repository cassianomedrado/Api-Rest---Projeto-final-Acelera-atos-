using Microsoft.EntityFrameworkCore;
using netbullAPI.Entidade;
using netbullAPI.Interfaces;
using netbullAPI.Util;
using netbullAPI.ViewModels;

namespace netbullAPI.Persistencia
{
    public class DAO_Endereco : DaoBase
    {
        private netbullDBContext _netbullDBContext;
        public DAO_Endereco(INotificador notificador, IConfiguration configuration, netbullDBContext netbullDBContext) : base(notificador, configuration)
        {
            _netbullDBContext = netbullDBContext;
        }

        public async Task<IEnumerable<Endereco>> BuscaEnderecosPessoaAsync(int idPessoa)
        {
            return await Task.FromResult(await _netbullDBContext.Enderecos.Where(x => x.endereco_idpessoa == idPessoa).ToListAsync());
        }

        public async Task<bool> CadastrarNovoEnderecoAsync(RegistrarEnderecoViewModel novoEnderecoViewModel)
        {
            try
            {
                int encontrouEndereco = (from end in _netbullDBContext.Enderecos
                                         where end.endereco_idpessoa == novoEnderecoViewModel.endereco_idpessoa
                                         && end.endereco_logradouro == novoEnderecoViewModel.endereco_logradouro
                                         && end.endereco_numero == novoEnderecoViewModel.endereco_numero
                                         && end.endereco_complemento == novoEnderecoViewModel.endereco_complemento
                                         select end).Count();
                var pessoaExiste = _netbullDBContext.Pessoas.Where(p => p.pessoa_id == novoEnderecoViewModel.endereco_idpessoa).FirstOrDefault();

                if(pessoaExiste == null)
                {
                    Notificar("ID de pessoa não cadastrado.");
                    return false;
                }

                if (encontrouEndereco != 0)
                {
                    Notificar("Endereço já cadastrado.");
                    return false;
                }
                using (_netbullDBContext)
                {
                    Endereco novoEndereco = new Endereco()
                    {
                        endereco_logradouro = novoEnderecoViewModel.endereco_logradouro,
                        endereco_numero = novoEnderecoViewModel.endereco_numero,
                        endereco_complemento = novoEnderecoViewModel.endereco_complemento,
                        endereco_idpessoa = novoEnderecoViewModel.endereco_idpessoa
                    };
                    _netbullDBContext.Enderecos.Add(novoEndereco);
                    await _netbullDBContext.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public async Task<bool> AtualizaEnderecoAsync(AlterarEnderecoViewModel attEndereco, int idEndereco)
        {
            try
            {
                //var enderecoExistente = _netbullDBContext.Enderecos.Where(x => x.endereco_id == attEndereco.endereco_id).FirstOrDefault();
                var enderecoExistente = (from e in _netbullDBContext.Enderecos
                                         where e.endereco_id == idEndereco
                                         select e).FirstOrDefault();
                if (enderecoExistente != null)
                {
                    using (_netbullDBContext)
                    {
                        enderecoExistente.endereco_logradouro = attEndereco.endereco_logradouro == "" ? enderecoExistente.endereco_logradouro : attEndereco.endereco_logradouro;
                        enderecoExistente.endereco_numero = (attEndereco.endereco_numero == 0 ? enderecoExistente.endereco_numero : attEndereco.endereco_numero);
                        enderecoExistente.endereco_complemento = (attEndereco.endereco_complemento == "" ? enderecoExistente.endereco_complemento : attEndereco.endereco_complemento);
                        await _netbullDBContext.SaveChangesAsync();
                        return true;
                    }
                }
                Notificar("Endereço não encontrado");
                return false;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public async Task<bool> AtualizaEnderecoLogradouroPatchAsync(int idEndereco, string logradouro)
        {
            try
            {
                var enderecoExistente = (from e in _netbullDBContext.Enderecos
                                         where e.endereco_id == idEndereco
                                         select e).FirstOrDefault();
                if (enderecoExistente == null)
                {
                    Notificar("Endereço informado não existe.");
                    return false;
                }

                using (_netbullDBContext)
                {
                    enderecoExistente.endereco_logradouro = logradouro;
                    await _netbullDBContext.SaveChangesAsync();
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public async Task<bool> ApagaEnderecoAsync(int idEndereco)
        {
            try
            {
                Endereco endereco = _netbullDBContext.Enderecos.Where(x => x.endereco_id == idEndereco).FirstOrDefault();
                if (endereco == null)
                {
                    Notificar("Endereço não encontrado.");
                    return false;
                }
                else
                {
                    _netbullDBContext.Enderecos.Remove(endereco);
                    await _netbullDBContext.SaveChangesAsync();
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}