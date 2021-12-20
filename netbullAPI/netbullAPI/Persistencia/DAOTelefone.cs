using netbullAPI.Entidade;
using netbullAPI.Interfaces;
using netbullAPI.Util;
using netbullAPI.ViewModels;

namespace netbullAPI.Persistencia
{
    public class DAOTelefone : DaoBase
    {
        private netbullDBContext _netbullDBContext;
        public DAOTelefone(INotificador notificador, IConfiguration configuration, netbullDBContext netbullDBContext) : base(notificador, configuration)      
        {
            _netbullDBContext = netbullDBContext;
        }

        public IEnumerable<Telefone> BuscaTelefoneCliente(int id)
        {
            try
            {
                var pessoa = _netbullDBContext.Pessoas.Where(p => p.pessoa_id == id).FirstOrDefault();
                //Verifica se o cliente informado ja existe no banco
                if (pessoa == null)
                {
                    Notificar("Cliente informado inexistente");
                    return null;
                }
                else
                {
                    var tels = from telefone in _netbullDBContext.Telefones
                               where telefone.telefone_idPessoa == id
                               select telefone;
                    return tels;
                }                
            }
            catch (Exception e)
            {
                Notificar(e.Message);
                throw e;
            }
        }

        public Telefone AdicionaTelefone(RegistrarTelefoneViewModel registrarTelefoneViewModel)
        {
            try
            {
                var pessoa = _netbullDBContext.Pessoas.Where(p => p.pessoa_id == registrarTelefoneViewModel.telefone_idPessoa).FirstOrDefault();
                //Verifica se o cliente informado ja existe no banco
                if (pessoa == null)
                {
                    Notificar("Cliente informado inexistente");
                    return null;
                }

                if (ValidaTelefone(registrarTelefoneViewModel.telefone_numero))
                {
                    var novoTelefone = new Telefone()
                    {
                        //Criado id dessa forma pois não tem autoincremento implementado
                        telefone_id = _netbullDBContext.Telefones.Max(m => m.telefone_id) + 1,
                        telefone_idPessoa = registrarTelefoneViewModel.telefone_idPessoa,
                        telefone_numero = registrarTelefoneViewModel.telefone_numero
                    };

                    _netbullDBContext.Add(novoTelefone);
                    _netbullDBContext.SaveChanges();
                    return novoTelefone;
                }
                else
                {
                    Notificar("Formatação inválida");
                    return null;
                } 
            }
            catch (Exception e)
            {
                Notificar(e.Message);
                throw e;
            }
        }

        public bool AtualizaTelefone(Telefone telefone)
        {
            try
            {
                var pessoa = _netbullDBContext.Pessoas.Where(x => x.pessoa_id == telefone.telefone_idPessoa).FirstOrDefault();
                //Verifica se o cliente informado ja existe no banco
                if (pessoa == null)
                {
                    Notificar("Cliente informado inexistente");
                    return false;
                }
                var telefoneExistente = _netbullDBContext.Telefones.Where(t => t.telefone_id == telefone.telefone_id).FirstOrDefault();
                if (telefoneExistente == null)
                {
                    Notificar("Telefone informado inexistente");
                    return false;
                }

                if (telefoneExistente.telefone_idPessoa != telefone.telefone_idPessoa)
                {
                    Notificar("Pessoa registrada para este telefone é diferente da informada");
                    return false;
                }

                if (ValidaTelefone(telefone.telefone_numero))
                {
                    telefoneExistente.telefone_numero = telefone.telefone_numero;
                    _netbullDBContext.Update(telefoneExistente);
                    _netbullDBContext.SaveChanges();
                    return true;
                }
                else
                    return false;
                
            }
            catch (Exception e)
            {
                Notificar(e.Message);
                throw e;
            }
        }

        private bool ValidaTelefone(int telefone)
        {
            if (telefone == 0)
            {
                Notificar("Valor de telefone informado inválido");
                return false;
            }
            else if (telefone.ToString().Length < 8)
            {
                Notificar("Formatação de telefone informada é inválida");
                return false;
            }

            return true;
        }

        public bool DeletaTelefone(int id)
        {
            try
            {
                var telefoneExistente = _netbullDBContext.Telefones.Where(t => t.telefone_id == id).FirstOrDefault();
                if (telefoneExistente == null)
                {
                    Notificar("Telefone informado inexistente");
                    return false;
                }
                _netbullDBContext.Remove(telefoneExistente);
                _netbullDBContext.SaveChanges();
                Notificar("Telefone deletado com sucesso");
                return true;
            }
            catch (Exception e)
            {
                Notificar(e.Message);
                throw e;
            }
        }
    }
}
