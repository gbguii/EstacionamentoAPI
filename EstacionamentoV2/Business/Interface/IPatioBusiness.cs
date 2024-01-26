using EstacionamentoV2.Business.DTO;
using EstacionamentoV2.Model;

namespace EstacionamentoV2.Business.Interface;

public interface IPatioBusiness
{
    public Task<GenericResponse> RecuperaPatios();
    public Task<GenericResponse> RecuperaPatioPorId(int id);
    public Task<GenericResponse> AtualizaPatio(AtualizarPatioDTO patio);
    public  Task<GenericResponse> CadastraPatio(CadastrarPatioDTO patio);
    public Task<GenericResponse> DeletaPatio(int id);
}
