using System.ComponentModel.DataAnnotations;

namespace EstacionamentoV2.Model ;

public class UsuarioModel
{
    [Key]
    public int Id { get; set; }
    public string Login { get; set; }
    public string Senha { get; set; }
    public string Acesso { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataAlteracao { get; set; }
    public bool Ativo { get; set; }
}
