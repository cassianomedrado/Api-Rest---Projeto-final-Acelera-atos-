using netbullAPI.Entidade;
using netbullAPI.Persistencia;
using netbullAPI.ViewModels;

namespace netbullAPI.MidwareDB
{
    public class NE_Telefone
    {
        private DAOTelefone _daoTelefone;
        public NE_Telefone(DAOTelefone daoTelefone)
        {
            _daoTelefone = daoTelefone;
        }

        public IEnumerable<Telefone>  BuscaTelefoneCliente(int id)
        {
            return _daoTelefone.BuscaTelefoneCliente(id);            
        }

        public Telefone AdicionaTelefone(RegistrarTelefoneViewModel registrarTelefoneViewModel)
        {
            return _daoTelefone.AdicionaTelefone(registrarTelefoneViewModel);
        }

        public bool AtualizaTelefone(Telefone telefone)
        {
            return _daoTelefone.AtualizaTelefone(telefone);
        }

        public bool DeletaTelefone(int id)
        {
            return _daoTelefone.DeletaTelefone(id);
        }
    }
}
