using EstacionamentoV2.Model;

namespace EstacionamentoV2.Repository.Interface;

public interface IVeiculoRepository
{
    public Task<List<VeiculoModel>> BuscaTodosVeiculos();
    public Task<VeiculoModel> BuscaVeiculoPorPlaca(string placa);
    public Task<VeiculoModel> BuscaVeiculoPorId(int id);
    public Task CadastraVeiculo(VeiculoModel veiculo);
    public Task AtualizaVeiculo(VeiculoModel veiculo);
    public Task DeletaVeiculo(VeiculoModel veiculo);
}
