using System.ComponentModel.DataAnnotations;

namespace netbullAPI.Entidade
{
    public class Pessoa
    {
        [Key]
        [Required]
        public int pessoa_id { get; set; }
        [Required]
        public int pessoa_documento { get; set; }
        public string pessoa_nome { get; set; }
        public EnumTipoPessoa pessoa_tipopessoa { get; set; }
    }
}
