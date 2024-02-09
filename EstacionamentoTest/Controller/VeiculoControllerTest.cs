using EstacionamentoV2.Business.DTO;
using EstacionamentoV2.Business.Interface;
using EstacionamentoV2.Controller;
using EstacionamentoV2.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EstacionamentoTest.Controllers;

public class VeiculoControllerTest
{
    private readonly VeiculoController _veiculoController;
    private readonly Mock<IVeiculoBusiness> _veiculoBusiness;
    public VeiculoControllerTest()
    {
        _veiculoBusiness = new Mock<IVeiculoBusiness>();
        _veiculoController = new VeiculoController(_veiculoBusiness.Object);
    }

    [Fact]
    public async void BuscaTodosVeiculos_ComSucesso()
    {
        // Arrange
        List<VeiculoModel> veiculos = new();
        GenericResponse retornoEsperado = new (){ Success = true, Data = veiculos, Message = "Veículos encontrados com sucesso" };
        _veiculoBusiness.Setup(x => x.BuscaTodosVeiculos()).ReturnsAsync(retornoEsperado);

        // Act
        IActionResult retorno = await _veiculoController.BuscaTodosVeiculo();

        // Assert
        Assert.Multiple(() =>
        {
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(retorno);
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal(veiculos, okObjectResult.Value);
        });
    }

    [Fact]
    public async void BuscaTodosVeiculos_SemSucesso_NenhumVeiculoEncontrado()
    {
        // Arrange
        GenericResponse retornoEsperado = new (){ Success = false, Message = "Nenhum veículo encontrado!" };
        _veiculoBusiness.Setup(x => x.BuscaTodosVeiculos()).ReturnsAsync(retornoEsperado);

        // Act
        IActionResult retorno = await _veiculoController.BuscaTodosVeiculo();

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.Equal("Nenhum veículo encontrado!", badRequestObjectResult.Value);
        });
    }

    [Fact]
    public async void BuscaVeiculoPorId_ComSucesso()
    {
        // Arrange
        VeiculoModel veiculo = new();
        GenericResponse retornoEsperado = new (){ Success = true, Data = veiculo, Message = "Veículo encontrado com sucesso" };
        _veiculoBusiness.Setup(x => x.BuscaVeiculoPorId(It.IsAny<int>())).ReturnsAsync(retornoEsperado);

        // Act
        IActionResult retorno = await _veiculoController.BuscaVeiculoPorId(1);

        // Assert
        Assert.Multiple(() =>
        {
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(retorno);
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal(veiculo, okObjectResult.Value);
        });
    }

    [Fact]
    public async void BuscaVeiculoPorId_SemSucesso_IdInvalido()
    {
        // Arrange
        GenericResponse retornoEsperado = new (){ Success = false, Message = "Id invalido" };
        _veiculoBusiness.Setup(x => x.BuscaVeiculoPorId(It.IsAny<int>())).ReturnsAsync(retornoEsperado);

        // Act
        IActionResult retorno = await _veiculoController.BuscaVeiculoPorId(0);

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.Equal("Id invalido", badRequestObjectResult.Value);
        });
    }

    [Fact]
    public async void BuscaVeiculoPorPlaca_ComSucesso()
    {
        // Arrange
        VeiculoModel veiculo = new();
        GenericResponse retornoEsperado = new (){ Success = true, Data = veiculo, Message = "Veículo encontrado com sucesso" };
        _veiculoBusiness.Setup(x => x.BuscaVeiculoPorPlaca(It.IsAny<string>())).ReturnsAsync(retornoEsperado);

        // Act
        IActionResult retorno = await _veiculoController.BuscaVeiculoPorPlaca("ABC1234");

        // Assert
        Assert.Multiple(() =>
        {
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(retorno);
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal(veiculo, okObjectResult.Value);
        });
    }

    [Fact]
    public async void BuscaVeiculoPorPlaca_SemSucesso_PlacaVazia()
    {
        // Arrange
        GenericResponse retornoEsperado = new (){ Success = false, Message = "Placa invalida" };
        _veiculoBusiness.Setup(x => x.BuscaVeiculoPorPlaca(It.IsAny<string>())).ReturnsAsync(retornoEsperado);

        // Act
        IActionResult retorno = await _veiculoController.BuscaVeiculoPorPlaca("");

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.Equal("Placa invalida", badRequestObjectResult.Value);
        });
    }

    [Fact]
    public async void CadastrarVeiculo_ComSucesso()
    {
        // Arrange
        CadastrarVeiculoDTO veiculo = new(){ Placa = "ABC1234", Modelo = "Gol", Tipo = '1'};
        GenericResponse retornoEsperado = new (){ Success = true, Message = "Veículo cadastrado com sucesso" };
        _veiculoBusiness.Setup(x => x.CadastraVeiculo(It.IsAny<CadastrarVeiculoDTO>())).ReturnsAsync(retornoEsperado);

        // Act
        IActionResult retorno = await _veiculoController.CadastrarVeiculo(veiculo);

        // Assert
        Assert.Multiple(() =>
        {
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(retorno);
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal("Veículo cadastrado com sucesso", okObjectResult.Value);
        });
    }

    [Fact]
    public async void CadastrarVeiculo_SemSucesso_CorpoRequisicaoInvalido()
    {
        // Arrange
        GenericResponse retornoEsperado = new (){ Success = false, Message = "Corpo da requisição invalido" };
        _veiculoBusiness.Setup(x => x.CadastraVeiculo(It.IsAny<CadastrarVeiculoDTO>())).ReturnsAsync(retornoEsperado);

        // Act
        IActionResult retorno = await _veiculoController.CadastrarVeiculo(null);

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.Equal("Corpo da requisição invalido", badRequestObjectResult.Value);
        });
    }

    [Fact]
    public async void CadastrarVeiculo_SemSucesso_PlacaInvalida()
    {
        // Arrange
        GenericResponse retornoEsperado = new (){ Success = false, Message = "Placa invalida" };
        _veiculoBusiness.Setup(x => x.CadastraVeiculo(It.IsAny<CadastrarVeiculoDTO>())).ReturnsAsync(retornoEsperado);

        // Act
        IActionResult retorno = await _veiculoController.CadastrarVeiculo(new CadastrarVeiculoDTO { Placa = "ABC12345", Modelo = "Gol", Tipo = '1' });

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.Equal("Placa invalida", badRequestObjectResult.Value);
        });
    }

    [Fact]
    public async void CadastrarVeiculo_SemSucesso_ModeloInvalido()
    {
        // Arrange
        GenericResponse retornoEsperado = new (){ Success = false, Message = "Modelo invalido" };
        _veiculoBusiness.Setup(x => x.CadastraVeiculo(It.IsAny<CadastrarVeiculoDTO>())).ReturnsAsync(retornoEsperado);

        // Act
        IActionResult retorno = await _veiculoController.CadastrarVeiculo(new CadastrarVeiculoDTO { Placa = "ABC1234" });

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.Equal("Modelo invalido", badRequestObjectResult.Value);
        });
    }

    [Fact]
    public async void CadastrarVeiculo_SemSucesso_TipoInvalido()
    {
        // Act
        IActionResult retorno = await _veiculoController.CadastrarVeiculo(new CadastrarVeiculoDTO { Placa = "ABC1234", Modelo = "Gol" });

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.Equal("Tipo invalido", badRequestObjectResult.Value);
        });
    }

    [Fact]
    public async void AtualizarVeiculo_ComSucesso()
    {
        // Arrange
        AtualizarVeiculoDTO veiculo = new(){ VeiculoID = 1, Placa = "ABC1234", Modelo = "Gol", Tipo = "1"};
        GenericResponse retornoEsperado = new (){ Success = true, Message = "Veículo atualizado com sucesso" };
        _veiculoBusiness.Setup(x => x.AtualizaVeiculo(It.IsAny<AtualizarVeiculoDTO>())).ReturnsAsync(retornoEsperado);

        // Act
        IActionResult retorno = await _veiculoController.AtualizarVeiculo(veiculo);

        // Assert
        Assert.Multiple(() =>
        {
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(retorno);
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal("Veículo atualizado com sucesso", okObjectResult.Value);
        });
    }

    [Fact]
    public async void AtualizarVeiculo_SemSucesso_CorpoRequisicaoInvalido()
    {
        // Arrange
        GenericResponse retornoEsperado = new (){ Success = false, Message = "Corpo da requisição invalido" };
        _veiculoBusiness.Setup(x => x.AtualizaVeiculo(It.IsAny<AtualizarVeiculoDTO>())).ReturnsAsync(retornoEsperado);

        // Act
        IActionResult retorno = await _veiculoController.AtualizarVeiculo(null);

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.Equal("Corpo da requisição invalido", badRequestObjectResult.Value);
        });
    }

    [Fact]
    public async void AtualizarVeiculo_SemSucesso_IdInvalido()
    {
        // Arrange
        GenericResponse retornoEsperado = new (){ Success = false, Message = "Id invalido" };
        _veiculoBusiness.Setup(x => x.AtualizaVeiculo(It.IsAny<AtualizarVeiculoDTO>())).ReturnsAsync(retornoEsperado);

        // Act
        IActionResult retorno = await _veiculoController.AtualizarVeiculo(new AtualizarVeiculoDTO { VeiculoID = 0 });

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.Equal("Id invalido", badRequestObjectResult.Value);
        });
    }

    [Fact]
    public async void AtualizarVeiculo_SemSucesso_PlacaInvalida()
    {

        // Act
        IActionResult retorno = await _veiculoController.AtualizarVeiculo(new AtualizarVeiculoDTO { VeiculoID = 1, Placa = "ABC12345", Modelo = "Gol", Tipo = "1"});

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.Equal("Placa invalida", badRequestObjectResult.Value);
        });
    }

    [Fact]
    public async void AtualizarVeiculo_SemSucesso_ModeloInvalido()
    {
        // Act
        IActionResult retorno = await _veiculoController.AtualizarVeiculo(new AtualizarVeiculoDTO { VeiculoID = 1, Placa = "ABC1234" });

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.Equal("Modelo invalido", badRequestObjectResult.Value);
        });
    }

    [Fact]
    public async void AtualizarVeiculo_SemSucesso_TipoInvalido()
    {
        // Act
        IActionResult retorno = await _veiculoController.AtualizarVeiculo(new AtualizarVeiculoDTO { VeiculoID = 1, Placa = "ABC1234", Modelo = "Gol"});

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.Equal("Tipo invalido", badRequestObjectResult.Value);
        });
    }

    [Fact]
    public async void AtualizarVeiculo_SemSucesso_VeiculoNaoEncontrado()
    {
        // Arrange
        GenericResponse retornoEsperado = new (){ Success = false, Message = "Veículo não encontrado" };
        _veiculoBusiness.Setup(x => x.AtualizaVeiculo(It.IsAny<AtualizarVeiculoDTO>())).ReturnsAsync(retornoEsperado);

        // Act
        IActionResult retorno = await _veiculoController.AtualizarVeiculo(new AtualizarVeiculoDTO { VeiculoID = 1, Placa = "ABC1234", Modelo = "Gol", Tipo = "1" });

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.Equal("Veículo não encontrado", badRequestObjectResult.Value);
        });
    }


    [Fact]
    public async void DeletarVeiculo_ComSucesso()
    {
        // Arrange
        GenericResponse retornoEsperado = new (){ Success = true, Message = "Veículo deletado com sucesso" };
        _veiculoBusiness.Setup(x => x.DeletaVeiculoPorId(It.IsAny<int>())).ReturnsAsync(retornoEsperado);

        // Act
        IActionResult retorno = await _veiculoController.DeletarVeiculo(1);

        // Assert
        Assert.Multiple(() =>
        {
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(retorno);
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal("Veículo deletado com sucesso", okObjectResult.Value);
        });
    }

    [Fact]
    public async void DeletarVeiculo_SemSucesso_IdInvalido()
    {
        // Act
        IActionResult retorno = await _veiculoController.DeletarVeiculo(0);

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.Equal("Id invalido", badRequestObjectResult.Value);
        });
    }

    [Fact]
    public async void DeletarVeiculo_SemSucesso_VeiculoNaoEncontrado()
    {
        // Arrange
        GenericResponse retornoEsperado = new (){ Success = false, Message = "Veículo não encontrado" };
        _veiculoBusiness.Setup(x => x.DeletaVeiculoPorId(It.IsAny<int>())).ReturnsAsync(retornoEsperado);

        // Act
        IActionResult retorno = await _veiculoController.DeletarVeiculo(1);

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.Equal("Veículo não encontrado", badRequestObjectResult.Value);
        });
    }

    [Fact]
    public async void DeletarVeiculoPorPlaca_ComSucesso()
    {
        // Arrange
        GenericResponse retornoEsperado = new (){ Success = true, Message = "Veículo deletado com sucesso" };
        _veiculoBusiness.Setup(x => x.DeletaVeiculoPorPlaca(It.IsAny<string>())).ReturnsAsync(retornoEsperado);

        // Act
        IActionResult retorno = await _veiculoController.DeletarVeiculoPorPlaca("ABC1234");

        // Assert
        Assert.Multiple(() =>
        {
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(retorno);
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal("Veículo deletado com sucesso", okObjectResult.Value);
        });
    }

    [Fact]
    public async void DeletarVeiculoPorPlaca_SemSucesso_PlacaInvalida()
    {
        // Act
        IActionResult retorno = await _veiculoController.DeletarVeiculoPorPlaca("");

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.Equal("Placa invalida", badRequestObjectResult.Value);
        });
    }

    [Fact]
    public async void DeletarVeiculoPorPlaca_SemSucesso_VeiculoNaoEncontrado()
    {
        // Arrange
        GenericResponse retornoEsperado = new (){ Success = false, Message = "Veículo não encontrado" };
        _veiculoBusiness.Setup(x => x.DeletaVeiculoPorPlaca(It.IsAny<string>())).ReturnsAsync(retornoEsperado);

        // Act
        IActionResult retorno = await _veiculoController.DeletarVeiculoPorPlaca("ABC1234");

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
            Assert.Equal("Veículo não encontrado", badRequestObjectResult.Value);
        });
    }
}
