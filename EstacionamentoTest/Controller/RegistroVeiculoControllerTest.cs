using EstacionamentoTest.Business;
using EstacionamentoV2.Business.DTO;
using EstacionamentoV2.Business.Interface;
using EstacionamentoV2.Controller;
using EstacionamentoV2.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EstacionamentoTest.Controllers;

public class RegistroVeiculoControllerTest
{
    private readonly Mock<IRegistroVeiculoBusiness> _registroVeiculoBusiness;
    private readonly RegistroVeiculoController _registroVeiculoController;

    public RegistroVeiculoControllerTest()
    {
        _registroVeiculoBusiness = new Mock<IRegistroVeiculoBusiness>();
        _registroVeiculoController = new RegistroVeiculoController(_registroVeiculoBusiness.Object);
    }

    [Fact]
    public async void ConsultaTodosVeiculosEstaionados_ComSucesso()
    {
        // Arrange
        List<RegistroVeiculoModel> veiculos = new(){new RegistroVeiculoModel(){Placa = "ABC1234", Modelo = "Gol", PatioID = 1, Mensalista = true}};
        _registroVeiculoBusiness.Setup(x => x.ConsultarVeiculoEstacionado()).ReturnsAsync(new GenericResponse(){Success = true, Data = veiculos});

        // Act
        IActionResult retorno = await _registroVeiculoController.ConsultarVeiculoEstacionado();

        // Assert
        Assert.Multiple(() =>
        {
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(retorno);
            Assert.IsType<List<RegistroVeiculoModel>>(okResult.Value);
            Assert.Equal(veiculos, okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
        });
    }

    [Fact]
    public async void ConsultaTodosVeiculosEstaionados_SemSucesso_NenhumVeiculoEstacionado()
    {
        // Arrange
        _registroVeiculoBusiness.Setup(x => x.ConsultarVeiculoEstacionado()).ReturnsAsync(new GenericResponse(){Success = false, Message = "Nenhum Veiculo Estacionado"});

        // Act
        IActionResult retorno = await _registroVeiculoController.ConsultarVeiculoEstacionado();

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Nenhum Veiculo Estacionado", badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        });
    }

    [Fact]
    public async void RegistrarEntradaVeiculo_ComSucesso()
    {
        // Arrange
        RegistrarEntradaVeiculoDTO veiculo = new(){Placa = "ABC1234", Modelo = "Gol", PatioId = 1, Mensalista = true};
        _registroVeiculoBusiness.Setup(x => x.RegistrarEntradaVeiculo(veiculo)).ReturnsAsync(new GenericResponse(){Success = true, Message = "Veiculo Registrado com Sucesso"});

        // Act
        IActionResult retorno = await _registroVeiculoController.RegistrarEntradaVeiculo(veiculo);

        // Assert
        Assert.Multiple(() =>
        {
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(retorno);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal("Veiculo Registrado com Sucesso", okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
        });
    }

    [Fact]
    public async void RegistrarEntradaVeiculo_SemSucesso_PlacaInvalida()
    {
        // Arrange
        RegistrarEntradaVeiculoDTO veiculo = new(){Modelo = "Gol", PatioId = 1, Mensalista = true};

        // Act
        IActionResult retorno = await _registroVeiculoController.RegistrarEntradaVeiculo(veiculo);

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Placa Inválida!", badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        });
    }

    [Fact]
    public async void RegistrarEntradaVeiculo_SemSucesso_ModeloVazio()
    {
        // Arrange
        RegistrarEntradaVeiculoDTO veiculo = new(){Placa = "ABC1234", PatioId = 1, Mensalista = true};

        // Act
        IActionResult retorno = await _registroVeiculoController.RegistrarEntradaVeiculo(veiculo);

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Modelo Inválido!", badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        });
    }

    [Fact]
    public async void RegistraEntradaVeiculo_SemSucesso_PatioIDNulo()
    {
        // Arrange
        RegistrarEntradaVeiculoDTO veiculo = new(){Placa = "ABC1234", Modelo = "Gol", Mensalista = true};

        // Act
        IActionResult retorno = await _registroVeiculoController.RegistrarEntradaVeiculo(veiculo);

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Patio Inválido!", badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        });
    }

    [Fact]
    public async void RegistraEntradaVeiculo_SemSucesso_PatioIDIgualZero()
    {
        // Arrange
        RegistrarEntradaVeiculoDTO veiculo = new(){Placa = "ABC1234", Modelo = "Gol", Mensalista = true, PatioId = 0};

        // Act
        IActionResult retorno = await _registroVeiculoController.RegistrarEntradaVeiculo(veiculo);

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Patio Inválido!", badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        });
    }

    [Fact]
    public async void RegistrarSaidaVeiculo_ComSucesso()
    {
        // Arrange
        RegistrarSaidaVeiculoDTO veiculo = new(){Placa = "ABC1234", Ticket = 1234567890};
        _registroVeiculoBusiness.Setup(x => x.RegistrarSaidaVeiculo(veiculo)).ReturnsAsync(new GenericResponse(){Success = true, Message = "Veiculo liberado com sucesso!"});

        // Act
        IActionResult retorno = await _registroVeiculoController.RegistrarSaidaVeiculo(veiculo);

        // Assert
        Assert.Multiple(() =>
        {
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(retorno);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal("Veiculo liberado com sucesso!", okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
        });
    }

    [Fact]
    public async void RegistrarSaidaVeiculo_SemSucesso_PlacaInvalida()
    {
        // Arrange
        RegistrarSaidaVeiculoDTO veiculo = new(){Ticket = 1234567890};

        // Act
        IActionResult retorno = await _registroVeiculoController.RegistrarSaidaVeiculo(veiculo);

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Placa Inválida!", badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        });
    }

    [Fact]
    public async void RegistrarSaidaVeiculo_SemSucesso_TicketInvalido()
    {
        // Arrange
        RegistrarSaidaVeiculoDTO veiculo = new(){Placa = "ABC1234"};

        // Act
        IActionResult retorno = await _registroVeiculoController.RegistrarSaidaVeiculo(veiculo);

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Ticket Inválido!", badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        });
    }

    [Fact]
    public async void RegistraSaidaVeicula_SemSucesso_VeiculoNaoEncontrado()
    {
        // Arrange
        RegistrarSaidaVeiculoDTO veiculo = new(){Placa = "ABC1234", Ticket = 1234567890};
        _registroVeiculoBusiness.Setup(x => x.RegistrarSaidaVeiculo(veiculo)).ReturnsAsync(new GenericResponse(){Success = false, Message = "Veiculo não encontrado!"});

        // Act
        IActionResult retorno = await _registroVeiculoController.RegistrarSaidaVeiculo(veiculo);

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Veiculo não encontrado!", badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        });
    }
    
}
