using EstacionamentoV2.Context;
using EstacionamentoV2.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstacionamentoV2;
[ApiController]
[Route("[controller]")]
public class PatioController: ControllerBase
{
    private readonly EstacionamentoContext _context;
    public PatioController(EstacionamentoContext context)
    {
        _context = context;
    }

    [HttpGet(Name = "GetPatio")]
    public async Task<ActionResult<IEnumerable<PatioModel>>> Get()
    {
        return await _context.Patio.ToListAsync();
    }

}
