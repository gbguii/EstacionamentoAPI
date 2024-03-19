using EstacionamentoV2.Business.DTO;
using EstacionamentoV2.Business.Interface;
using EstacionamentoV2.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EstacionamentoV2.Controller;

public class UsuarioController : ControllerBase
{
    private readonly IUsuarioBusiness _usuarioBusiness;

    public UsuarioController(IUsuarioBusiness usuarioBusiness)
    {
        _usuarioBusiness = usuarioBusiness;
    }

    [Authorize]
    [HttpGet("RetornaUsuario/{login}/{senha}")]
    public async Task<IActionResult> RetornaUsuario([FromRoute] string login, [FromRoute] string senha)
    {
        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(senha))
        {
            return BadRequest("Login ou senha invalidos.");
        }
        GenericResponse retorno = await _usuarioBusiness.RetornaUsuario(login, senha);
        return retorno.Success ? Ok(retorno.Data) : BadRequest(retorno.Message);
    }

    [Authorize]
    [HttpGet("RetornaUsuarioPorLogin/{login}")]
    public async Task<IActionResult> RetornaUsuarioPorLogin([FromRoute] string login)
    {
        if (string.IsNullOrEmpty(login))
        {
            return BadRequest("Login invalido.");
        }
        GenericResponse retorno = await _usuarioBusiness.RetornaUsuarioPorLogin(login);
        return retorno.Success ? Ok(retorno.Data) : BadRequest(retorno.Message);
    }


    [Authorize]
    [HttpPost("CadastrarUsuario")]
    public async Task<IActionResult> CadastrarUsuario([FromBody] RegistrarUsuarioDTO usuario)
    {
        if (usuario == null)
        {
            return BadRequest("Corpo da requisição invalido.");
        }

        if (usuario.Login.Length < 3)
        {
            return BadRequest("Login invalido.");
        }

        if (usuario.Senha.Length < 5)
        {
            return BadRequest("Senha invalida.");
        }

        if (usuario.Acesso.Length < 2)
        {
            return BadRequest("Acesso invalido.");
        }


        GenericResponse retorno = await _usuarioBusiness.CriaUsuario(usuario);
        return retorno.Success ? Ok(retorno.Message) : BadRequest(retorno.Message);
    }
}
