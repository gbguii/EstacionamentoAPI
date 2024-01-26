using EstacionamentoV2.Business.DTO;
using EstacionamentoV2.Business.Interface;
using EstacionamentoV2.Model;
using Microsoft.AspNetCore.Mvc;

namespace EstacionamentoV2.Controller;

public class RegistroVeiculoController: ControllerBase
{
    private readonly IRegistroVeiculoBusiness _registroVeiculoBusiness;
    public RegistroVeiculoController(IRegistroVeiculoBusiness registroVeiculoBusiness)
    {
        _registroVeiculoBusiness = registroVeiculoBusiness;
    }

    
    [HttpGet("ConsultarVeiculosEstacionados")]
    public async Task<IActionResult> ConsultarVeiculoEstacionado()
    {
        GenericResponse retorno = await _registroVeiculoBusiness.ConsultarVeiculoEstacionado();
        return retorno.Success ? Ok(retorno.Data) : BadRequest(retorno.Message);
    }

    [HttpPost("RegistrarEntradaVeiculo")]
    public async Task<IActionResult> RegistrarEntradaVeiculo([FromBody] RegistrarEntradaVeiculoDTO veiculo)
    {
        GenericResponse response = await _registroVeiculoBusiness.RegistrarEntradaVeiculo(veiculo);
        if(response.Success)
        {
            return Ok(response.Message);
        }
        return BadRequest(response.Message);
    }

    [HttpPost("RegistrarSaidaVeiculo")]
    public async Task<IActionResult> RegistrarSaidaVeiculo([FromBody] RegistrarSaidaVeiculoDTO veiculo)
    {
        GenericResponse response = await _registroVeiculoBusiness.RegistrarSaidaVeiculo(veiculo);
        if(response.Success)
        {
            return Ok(response.Message);
        }
        return BadRequest(response.Message);
    }
}
