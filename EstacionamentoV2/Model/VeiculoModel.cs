using System.ComponentModel.DataAnnotations;

namespace EstacionamentoV2.Model;

public class VeiculoModel
{
    [Key]
    public int VeiculoID { get; set; }
    public string VeiculoPlaca { get; set; }
    public string VeiculoModelo { get; set; }
    public char VeiculoTipo { get; set; }
    public string? VeiculoProprietario { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataAtualizacao { get; set; }
}
