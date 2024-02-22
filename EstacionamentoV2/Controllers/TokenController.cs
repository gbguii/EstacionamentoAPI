using EstacionamentoV2.Business.DTO;
using EstacionamentoV2.Business.Interface;
using EstacionamentoV2.Model;
using Microsoft.AspNetCore.Mvc;

namespace EstacionamentoV2.Controller;

public class TokenController: ControllerBase
{
    ITokenBusiness _tokenBusiness;
    public TokenController(ITokenBusiness tokenBusiness)
    {
        _tokenBusiness = tokenBusiness;
    }

    [HttpPost("GerarToken")]
    public async Task<ActionResult> GerarToken([FromBody] UsuarioTokenDTO usuario)
    {
        GenericResponse retorno = await _tokenBusiness.GerarToken(usuario);
        return retorno.Success ? Ok(retorno.Data) : BadRequest(retorno.Message);
    }
}
