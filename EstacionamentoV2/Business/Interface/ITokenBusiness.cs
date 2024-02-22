using EstacionamentoV2.Business.DTO;
using EstacionamentoV2.Model;

namespace EstacionamentoV2.Business.Interface;

public interface ITokenBusiness
{
    public Task<GenericResponse> GerarToken(UsuarioTokenDTO usuario); 
}
