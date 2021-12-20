using System.ComponentModel.DataAnnotations.Schema;

namespace netbullAPI.ViewModels
{
    public class RegistrarTelefoneViewModel
    {
        public int telefone_numero { get; set; }
        [ForeignKey("Pessoa")]
        public int telefone_idPessoa { get; set; }
    }
}
