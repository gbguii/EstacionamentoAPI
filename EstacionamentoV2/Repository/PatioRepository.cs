using EstacionamentoV2.Context;
using EstacionamentoV2.Model;
using EstacionamentoV2.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace EstacionamentoV2.Repository;

public class PatioRepository: IPatioRepository
{
    private readonly EstacionamentoContext _context;
    public PatioRepository(EstacionamentoContext context)
    {
        _context = context;
    }

    public async Task<List<PatioModel>> RecuperaPatios() 
    {
        return await _context.Patio.ToListAsync();
    }

    public async Task<PatioModel> RecuperaPatioPorId(int id)
    {
        return await _context.Patio.FirstOrDefaultAsync(p => p.PatioID == id);
    }

    public async Task AtualizaPatio(PatioModel patio)
    {
        _context.Patio.Update(patio);
        await _context.SaveChangesAsync();
    }

    public async Task CadastraPatio(PatioModel patio)
    {
        await _context.Patio.AddAsync(patio);
        await _context.SaveChangesAsync();
    }

    public async Task DeletaPatio(PatioModel patio)
    {
        _context.Patio.Remove(patio);
        await _context.SaveChangesAsync();
    }
}
