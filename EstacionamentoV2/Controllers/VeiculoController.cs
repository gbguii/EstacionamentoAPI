using EstacionamentoV2.Business.DTO;
using EstacionamentoV2.Business.Interface;
using EstacionamentoV2.Model;
using Microsoft.AspNetCore.Mvc;

namespace EstacionamentoV2.Controller;

public class VeiculoController: ControllerBase
{
    private readonly IVeiculoBusiness _veiculoBusiness;
    public VeiculoController(IVeiculoBusiness veiculoBusiness)
    {
        _veiculoBusiness = veiculoBusiness;
    }

    [HttpGet("BuscaTodosVeiculos")]
    public async Task<IActionResult> BuscaTodosVeiculo()
    {
        GenericResponse retorno = await _veiculoBusiness.BuscaTodosVeiculos();
        return retorno.Success ? Ok(retorno.Data) : BadRequest(retorno.Message);
    }

    [HttpGet("BuscaVeiculoPorId/{id}")]
    public async Task<IActionResult> BuscaVeiculoPorId(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Id invalido");
        }
        GenericResponse retorno = await _veiculoBusiness.BuscaVeiculoPorId(id);
        return retorno.Success ? Ok(retorno.Data) : BadRequest(retorno.Message);
    }
    [HttpGet("BuscaVeiculoPorPlaca/{placa}")]
    public async Task<IActionResult> BuscaVeiculoPorPlaca(string placa)
    {
        if (string.IsNullOrEmpty(placa))
        {
            return BadRequest("Placa invalida");
        }
        GenericResponse retorno = await _veiculoBusiness.BuscaVeiculoPorPlaca(placa);
        return retorno.Success ? Ok(retorno.Data) : BadRequest(retorno.Message);
    }

    [HttpPost("CadastrarVeiculo")]
    public async Task<IActionResult> CadastrarVeiculo([FromBody] CadastrarVeiculoDTO veiculo)
    {
        if (veiculo == null)
        {
            return BadRequest("Corpo da requisição invalido");
        }

        if (veiculo.Placa.Length > 7)
        {
            return BadRequest("Placa invalida");
        }
        if (string.IsNullOrEmpty(veiculo.Modelo))
        {
            return BadRequest("Modelo invalido");
        }
        if (string.IsNullOrEmpty(veiculo.Tipo.ToString()) || char.MinValue == veiculo.Tipo)
        {
            return BadRequest("Tipo invalido");
        }
        GenericResponse retorno = await _veiculoBusiness.CadastraVeiculo(veiculo);
        return retorno.Success ? Ok(retorno.Message) : BadRequest(retorno.Message);
    }

    [HttpPut("AtualizarVeiculo")]
    public async Task<IActionResult> AtualizarVeiculo([FromBody] AtualizarVeiculoDTO veiculo)
    {
        if (veiculo == null)
        {
            return BadRequest("Corpo da requisição invalido");
        }

        if (veiculo.VeiculoID <= 0)
        {
            return BadRequest("Id invalido");
        }
        if (veiculo.Placa.Length > 7)
        {
            return BadRequest("Placa invalida");
        }
        if (string.IsNullOrEmpty(veiculo.Modelo))
        {
            return BadRequest("Modelo invalido");
        }
        if (string.IsNullOrEmpty(veiculo.Tipo))
        {
            return BadRequest("Tipo invalido");
        }
        GenericResponse retorno = await _veiculoBusiness.AtualizaVeiculo(veiculo);
        return retorno.Success ? Ok(retorno.Message) : BadRequest(retorno.Message);
    }

    [HttpDelete("DeletarVeiculo/{id}")]
    public async Task<IActionResult> DeletarVeiculo(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Id invalido");
        }
        GenericResponse retorno = await _veiculoBusiness.DeletaVeiculoPorId(id);
        return retorno.Success ? Ok(retorno.Message) : BadRequest(retorno.Message);
    }

    [HttpDelete("DeletarVeiculoPorPlaca/{placa}")]
    public async Task<IActionResult> DeletarVeiculoPorPlaca(string placa)
    {
        if (string.IsNullOrEmpty(placa))
        {
            return BadRequest("Placa invalida");
        }
        GenericResponse retorno = await _veiculoBusiness.DeletaVeiculoPorPlaca(placa);
        return retorno.Success ? Ok(retorno.Message) : BadRequest(retorno.Message);
    }
}
