using netbullAPI.Entidade;
using System.ComponentModel.DataAnnotations;

namespace netbullAPI.ViewModels
{
    public class CadastrarPessoaViewModel
    {
        [Required(ErrorMessage = "O parametro documento é obrigatório.")]
        public int pessoa_documento { get; set; }

        [Required(ErrorMessage = "O parametro nome é obrigatório.")]
        [StringLength(60, ErrorMessage = "O parametro nome deve ter no máximo 60 caracteres.")]
        public string pessoa_nome { get; set; }

        [Required(ErrorMessage = "O parametro tipo pessoa é obrigatório.")]
        public EnumTipoPessoa pessoa_tipopessoa { get; set; }

    }
}
