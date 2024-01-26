using EstacionamentoV2.Context;
using EstacionamentoV2.Model;
using EstacionamentoV2.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace EstacionamentoV2.Repository;

public class RegistroVeiculoRepository: IRegistroVeiculoRepository
{
    private readonly EstacionamentoContext _context;
    public RegistroVeiculoRepository(EstacionamentoContext context)
    {
        _context = context;
    }
    
    public async Task<RegistroVeiculoModel> ConsultarVeiculoEstacionado(string placa)
    {
        return await _context.RegistroVeiculo.FirstOrDefaultAsync(x => x.Placa == placa);
    }

    public Task<List<RegistroVeiculoModel>> ConsultarVeiculosEstacionados()
    {
        return _context.RegistroVeiculo.Where(x => x.DataSaida == null).ToListAsync();
    }

    public async Task RegistrarEntradaVeiculo(RegistroVeiculoModel veiculo)
    {
        _context.RegistroVeiculo.Add(veiculo);
        await _context.SaveChangesAsync();
    }

    public async Task RegistrarSaidaVeiculo(RegistroVeiculoModel veiculo)
    {
        _context.RegistroVeiculo.Update(veiculo);
        await _context.SaveChangesAsync();
    }
}
