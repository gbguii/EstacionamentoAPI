using EstacionamentoV2.Model;

namespace EstacionamentoV2.Repository.Interface;

public interface IRegistroVeiculoRepository
{
    public Task<List<RegistroVeiculoModel>> ConsultarVeiculosEstacionados();
    public Task RegistrarEntradaVeiculo(RegistroVeiculoModel veiculo);
    public Task<RegistroVeiculoModel> ConsultarVeiculoEstacionado(string placa);
    public Task RegistrarSaidaVeiculo(RegistroVeiculoModel veiculo);
}
