using System.ComponentModel.DataAnnotations;

namespace EstacionamentoV2.Model
{
    public class PatioModel
    {
        [Key]
        public int PatioID { get; set; }
        public string PatioNome { get; set; }
        public int PatioVagas { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
