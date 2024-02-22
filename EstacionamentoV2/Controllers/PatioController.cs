using EstacionamentoV2.Business.DTO;
using EstacionamentoV2.Business.Interface;
using EstacionamentoV2.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EstacionamentoV2.Controller;
[ApiController]
[Route("[controller]")]
public class PatioController: ControllerBase
{
    private readonly IPatioBusiness _patioBusiness;
    public PatioController(IPatioBusiness patioBusiness)
    {
        _patioBusiness = patioBusiness;
    }

    [Authorize]
    [HttpGet("RecuperaPatios")]
    public async Task<IActionResult> RecuperaPatios()
    {
        GenericResponse retorno = await _patioBusiness.RecuperaPatios();
        return retorno.Success ? Ok(retorno.Data) : BadRequest(retorno.Message);
    }

    [Authorize]
    [HttpGet("RecuperaPatio-PorId/{id}")]
    public async Task<IActionResult> RecuperaPatioPorId(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Id invalido!");
        }
        GenericResponse retorno = await _patioBusiness.RecuperaPatioPorId(id);
        return retorno.Success ? Ok(retorno.Data) : BadRequest(retorno.Message);
    }

    [Authorize]
    [HttpPut("AtualizaPatio")]
    public async Task<IActionResult> AtualizaPatio(AtualizarPatioDTO patio)
    {
        if (patio.PatioID <= 0 || patio.PatioNome == null || patio.PatioVagas <= 0)
        {
            return BadRequest("Dados invalidos!");
        }

        GenericResponse retorno = await _patioBusiness.AtualizaPatio(patio);
        return retorno.Success ? Ok(retorno.Message) : BadRequest(retorno.Message);
    }

    [Authorize]
    [HttpPost("AdicionaPatio")]
    public async Task<IActionResult> AdicionaPatio(CadastrarPatioDTO patio)
    {
        if (patio.PatioNome == null || patio.PatioVagas <= 0)
        {
            return BadRequest("Dados invalidos!");
        }

        GenericResponse retorno = await _patioBusiness.CadastraPatio(patio);
        return retorno.Success ? Ok(retorno.Message) : BadRequest(retorno.Message);
    }

    [Authorize]
    [HttpDelete("DeletaPatio/{id}")]
    public async Task<IActionResult> DeletaPatio(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Id invalido!");
        }

        GenericResponse retorno = await _patioBusiness.DeletaPatio(id);
        return retorno.Success ? Ok(retorno.Message) : BadRequest(retorno.Message);
    }
}
