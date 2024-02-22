using EstacionamentoV2.Business.DTO;
using EstacionamentoV2.Model;

namespace EstacionamentoV2.Business.Interface;

public interface IUsuarioBusiness
{
    public Task<GenericResponse> RetornaUsuario(string login, string senha);
    public Task<GenericResponse> RetornaUsuarioPorLogin(string login);
    public Task<GenericResponse> CriaUsuario(RegistrarUsuarioDTO usuario);
}
