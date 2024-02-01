using EstacionamentoV2.Model;
using EstacionamentoV2.Repository.Interface;

namespace EstacionamentoTest;

public class PatioRepositoryFake : IPatioRepository
{
    
    public Task<PatioModel> RecuperaPatioPorId(int id)
    {
        if (id == 1)
        {
            return Task.FromResult(
                new PatioModel
                {
                    PatioID = 1,
                    PatioNome = "Patio 1",
                    PatioVagas = 10,
                    DataCadastro = DateTime.Now,
                    DataAtualizacao = DateTime.Now
                }
            );
        }
        return Task.FromResult<PatioModel>(null);
    }

    public Task<List<PatioModel>> RecuperaPatios()
    {
        throw new NotImplementedException();
    }

    public Task AtualizaPatio(PatioModel patio)
    {
        throw new NotImplementedException();
    }

    public Task CadastraPatio(PatioModel patio)
    {
        throw new NotImplementedException();
    }

    public Task DeletaPatio(PatioModel patio)
    {
        throw new NotImplementedException();
    }
}
