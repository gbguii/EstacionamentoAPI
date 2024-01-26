using EstacionamentoV2.Context;
using EstacionamentoV2.Model;
using EstacionamentoV2.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace EstacionamentoV2.Repository;

public class VeiculoRepository: IVeiculoRepository
{
    private readonly EstacionamentoContext _context;
    public VeiculoRepository(EstacionamentoContext context)
    {
        _context = context;
    }

    public async Task<List<VeiculoModel>> BuscaTodosVeiculos()
    {
        return await _context.Veiculo.ToListAsync();
    }

    public async Task<VeiculoModel> BuscaVeiculoPorId(int id)
    {
        return await _context.Veiculo.FirstOrDefaultAsync(v => v.VeiculoID == id);
    }

    public async Task<VeiculoModel> BuscaVeiculoPorPlaca(string placa)
    {
        return await _context.Veiculo.FirstOrDefaultAsync(v => v.VeiculoPlaca == placa);
    }

    public async Task CadastraVeiculo(VeiculoModel veiculo)
    {
        await _context.Veiculo.AddAsync(veiculo);
        await _context.SaveChangesAsync();
    }
    public async Task AtualizaVeiculo(VeiculoModel veiculo)
    {
        _context.Veiculo.Update(veiculo);
        await _context.SaveChangesAsync();
    }

    public async Task DeletaVeiculo(VeiculoModel veiculo)
    {
        _context.Veiculo.Remove(veiculo);
        await _context.SaveChangesAsync();
    }
}
