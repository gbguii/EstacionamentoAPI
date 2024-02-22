using EstacionamentoV2.Business.DTO;
using EstacionamentoV2.Business.Interface;
using EstacionamentoV2.Model;
using EstacionamentoV2.Repository.Interface;

namespace EstacionamentoV2.Business;

public class UsuarioBusiness : IUsuarioBusiness
{
    IUsuarioRepository _usuarioRepository;
    public UsuarioBusiness(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<GenericResponse> RetornaUsuario(string login, string senha)
    {
        UsuarioModel usuario = await _usuarioRepository.RetornaUsuario(login, senha);
        if (usuario == null)
        {
            return new GenericResponse { Success = false, Message = "Usuário não encontrado.", Data = null };
        }
        return new GenericResponse { Success = true, Message = "Usuário retornado com sucesso.", Data = usuario };
    }

    public async Task<GenericResponse> RetornaUsuarioPorLogin(string login)
    {
        UsuarioModel usuario = await _usuarioRepository.RetornaUsuarioPorLogin(login);
        if (usuario == null)
        {
            return new GenericResponse { Success = false, Message = "Usuário não encontrado.", Data = null };
        }
        return new GenericResponse { Success = true, Message = "Usuário retornado com sucesso.", Data = usuario };
    }
    public async Task<GenericResponse> CriaUsuario(RegistrarUsuarioDTO usuario)
    {
        UsuarioModel usuarioExistente = await _usuarioRepository.RetornaUsuarioPorLogin(usuario.Login);
        if (usuarioExistente != null)
        {
            return new GenericResponse { Success = false, Message = "Login já existente.", Data = null };
        }

        UsuarioModel usuarioModel = new()
        {
            Login = usuario.Login,
            Senha = usuario.Senha,
            Acesso = usuario.Acesso,
            DataCriacao = DateTime.Now,
            DataAlteracao = DateTime.Now,
            Ativo = true
        };
        await _usuarioRepository.CriaUsuario(usuarioModel);
        return new GenericResponse { Success = true, Message = "Usuário criado com sucesso.", Data = null };
    }
}
