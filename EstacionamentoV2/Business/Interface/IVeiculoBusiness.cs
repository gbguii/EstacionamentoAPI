using EstacionamentoV2.Business.DTO;
using EstacionamentoV2.Model;

namespace EstacionamentoV2.Business.Interface;

public interface IVeiculoBusiness
{
    public Task<GenericResponse> BuscaTodosVeiculos();
    public Task<GenericResponse> BuscaVeiculoPorPlaca(string placa);
    public Task<GenericResponse> BuscaVeiculoPorId(int id);
    public Task<GenericResponse> CadastraVeiculo(CadastrarVeiculoDTO veiculo);
    public Task<GenericResponse> AtualizaVeiculo(AtualizarVeiculoDTO veiculo);
    public Task<GenericResponse> DeletaVeiculoPorId(int id);
    public Task<GenericResponse> DeletaVeiculoPorPlaca(string placa);
}
