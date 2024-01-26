using System.ComponentModel.DataAnnotations;

namespace EstacionamentoV2.Model;

public class RegistroVeiculoModel
{
    [Key]
    public int Id { get; set; }
    public string Placa { get; set; }
    public int? VeiculoId {get; set;}
    public string Modelo { get; set; }
    public bool Mensalista { get; set; }
    public DateTime DataEntrada { get; set; }
    public DateTime? DataSaida { get; set; }
    public double ValorPago { get; set; }
    public int PatioID { get; set; }
}
