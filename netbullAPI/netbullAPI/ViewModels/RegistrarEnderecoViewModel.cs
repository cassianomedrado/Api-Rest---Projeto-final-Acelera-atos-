using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace netbullAPI.ViewModels
{
    public class RegistrarEnderecoViewModel
    {
        [Required(ErrorMessage = "O logradouro é obrigatório,")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "O campo logradouro deve ter entre 10 e 50 caracteres.")]
        public string? endereco_logradouro { get; set; }
        [Required(ErrorMessage = "O número do endereço é obrigatório.")]
        public int endereco_numero { get; set; }
        public string endereco_complemento { get; set; }
        [ForeignKey("Pessoa")]
        public int endereco_idpessoa { get; set; }
    }
}
