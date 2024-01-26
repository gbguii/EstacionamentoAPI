using EstacionamentoV2.Business.DTO;
using EstacionamentoV2.Model;

namespace EstacionamentoV2.Business.Interface;

public interface IRegistroVeiculoBusiness
{
    public Task<GenericResponse> ConsultarVeiculoEstacionado();
    public Task<GenericResponse> RegistrarEntradaVeiculo(RegistrarEntradaVeiculoDTO veiculo);
    public Task<GenericResponse> RegistrarSaidaVeiculo(RegistrarSaidaVeiculoDTO veiculo);
}
