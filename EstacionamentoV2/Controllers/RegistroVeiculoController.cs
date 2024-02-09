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
        if(veiculo.Placa == null)
        {
            return BadRequest("Placa Inválida!");
        }
        if(veiculo.Modelo == null)
        {
            return BadRequest("Modelo Inválido!");
        }
        if(veiculo.PatioId == 0 || veiculo.PatioId <= 0)
        {
            return BadRequest("Patio Inválido!");
        }

        GenericResponse response = await _registroVeiculoBusiness.RegistrarEntradaVeiculo(veiculo);

        return response.Success ? Ok(response.Message) : BadRequest(response.Message);
    }

    [HttpPost("RegistrarSaidaVeiculo")]
    public async Task<IActionResult> RegistrarSaidaVeiculo([FromBody] RegistrarSaidaVeiculoDTO veiculo)
    {
        if(veiculo.Placa == null)
        {
            return BadRequest("Placa Inválida!");
        }
        if(veiculo.Ticket == 0)
        {
            return BadRequest("Ticket Inválido!");
        }

        GenericResponse response = await _registroVeiculoBusiness.RegistrarSaidaVeiculo(veiculo);

        return response.Success ? Ok(response.Message) : BadRequest(response.Message);
    }
}
