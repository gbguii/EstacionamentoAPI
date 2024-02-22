using EstacionamentoV2.Model;

namespace EstacionamentoV2.Repository.Interface;

public interface IUsuarioRepository
{
    public Task<UsuarioModel> RetornaUsuario(string login, string senha);
    public Task<UsuarioModel> RetornaUsuarioPorLogin(string login);
    public Task CriaUsuario(UsuarioModel usuario);
}
