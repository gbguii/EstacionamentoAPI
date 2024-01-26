using EstacionamentoV2.Model;

namespace EstacionamentoV2.Repository.Interface;

public interface IPatioRepository
{
    public Task<List<PatioModel>> RecuperaPatios();
    public Task<PatioModel> RecuperaPatioPorId(int id);
    public Task AtualizaPatio(PatioModel patio);
    public Task CadastraPatio(PatioModel patio);
    public Task DeletaPatio(PatioModel patio);
}
