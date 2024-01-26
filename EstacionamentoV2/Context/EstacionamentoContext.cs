using EstacionamentoV2.Model;
using Microsoft.EntityFrameworkCore;

namespace EstacionamentoV2.Context;

public class EstacionamentoContext: DbContext
{
    public EstacionamentoContext(DbContextOptions<EstacionamentoContext> options): base(options)
    {
        
    }

    public DbSet<PatioModel> Patio { get; set; }
    public DbSet<VeiculoModel> Veiculo { get; set; }
    public DbSet<RegistroVeiculoModel> RegistroVeiculo { get; set; }
}
