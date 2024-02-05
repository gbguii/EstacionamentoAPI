using EstacionamentoV2.Business;
using EstacionamentoV2.Business.DTO;
using EstacionamentoV2.Model;
using EstacionamentoV2.Repository.Interface;
using Moq;

namespace EstacionamentoTest.Business;

public class VeiculoBusinessTest
{
    private readonly VeiculoBusiness _veiculoBusiness;
    private readonly Mock<IVeiculoRepository> _repositoryMock;
    public VeiculoBusinessTest()
    {
        _repositoryMock = new();
        _veiculoBusiness = new(_repositoryMock.Object);
    }

    private readonly List<VeiculoModel> veiculos = new()
    {
        new VeiculoModel
        {
            VeiculoID = 1,
                VeiculoPlaca = "ABC1234",
                VeiculoModelo = "Fusca",
                VeiculoTipo = 'A',
                VeiculoProprietario = "Branco",
                DataCriacao = DateTime.Now,
                DataAtualizacao = DateTime.Now
        },
        new VeiculoModel
        {
                VeiculoID = 2,
                VeiculoPlaca = "DEF5678",
                VeiculoModelo = "Gol",
                VeiculoTipo = 'A',
                VeiculoProprietario = "Preto",
                DataCriacao = DateTime.Now,
                DataAtualizacao = DateTime.Now
        }
    };

    [Fact]
    public async void BuscaTodosVeiculos_DeveRetornarListaDeVeiculos()
    {
        // Arrange
        _repositoryMock.Setup(x => x.BuscaTodosVeiculos()).ReturnsAsync(veiculos);
        GenericResponse retornoEsperado = new() { Success = true, Message = "Veiculos encontrados com sucesso", Data = veiculos};

        // Act
        GenericResponse result = await _veiculoBusiness.BuscaTodosVeiculos();

        // Assert
        Assert.Equivalent(retornoEsperado, result);
    }

    [Fact]
    public async void BuscaTodosVeiculos_SemSucesso_NenhumVeiculoEncontradoS()
    {
        // Arrange
        _repositoryMock.Setup(x => x.BuscaTodosVeiculos()).ReturnsAsync(new List<VeiculoModel>());
        GenericResponse retornoEsperado = new() { Success = false, Message = "Nenhum veiculo encontrado", Data = null};

        // Act
        GenericResponse result = await _veiculoBusiness.BuscaTodosVeiculos();

        // Assert
        Assert.Equivalent(retornoEsperado, result);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async void BuscaVeiculoPorId_ComSucesso(int id)
    {
        // Arrange
        _repositoryMock.Setup(x => x.BuscaVeiculoPorId(id)).ReturnsAsync(veiculos[id - 1]);
        GenericResponse retornoEsperado = new() { Success = true, Message = "Veiculo encontrado com sucesso", Data = veiculos[id - 1]};

        // Act
        GenericResponse result = await _veiculoBusiness.BuscaVeiculoPorId(id);

        // Assert
        Assert.Equivalent(retornoEsperado, result);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(4)]
    public async void BuscaVeiculoPorId_SemSucesso(int id)
    {
        // Arrange
        _repositoryMock.Setup(x => x.BuscaVeiculoPorId(id)).ReturnsAsync((VeiculoModel)null);
        GenericResponse retornoEsperado = new() { Success = false, Message = "Veiculo não encontrado", Data = null};

        // Act
        GenericResponse result = await _veiculoBusiness.BuscaVeiculoPorId(id);

        // Assert
        Assert.Equivalent(retornoEsperado, result);
    }

    [Theory]
    [InlineData("ABC1234")]
    [InlineData("DEF5678")]
    public async void BuscaVeiculoPorPlaca_ComSucesso(string placa)
    {
        // Arrange
        _repositoryMock.Setup(x => x.BuscaVeiculoPorPlaca(placa)).ReturnsAsync(veiculos.Find(x => x.VeiculoPlaca == placa));
        GenericResponse retornoEsperado = new() { Success = true, Message = "Veiculo encontrado com sucesso", Data = veiculos.Find(x => x.VeiculoPlaca == placa)};

        // Act
        GenericResponse result = await _veiculoBusiness.BuscaVeiculoPorPlaca(placa);

        // Assert
        Assert.Equivalent(retornoEsperado, result);
    }

    [Theory]
    [InlineData("GHI9012")]
    [InlineData("JKL3456")]
    public async void BuscaVeiculoPorPlaca_SemSucesso(string placa)
    {
        // Arrange
        _repositoryMock.Setup(x => x.BuscaVeiculoPorPlaca(placa)).ReturnsAsync((VeiculoModel)null);
        GenericResponse retornoEsperado = new() { Success = false, Message = "Veiculo não encontrado", Data = null};

        // Act
        GenericResponse result = await _veiculoBusiness.BuscaVeiculoPorPlaca(placa);

        // Assert
        Assert.Equivalent(retornoEsperado, result);
    }

    [Fact]
    public async void CadastraVeiculo_ComSucesso()
    {
        // Arrange
        CadastrarVeiculoDTO veiculo = new()
        {
            Placa = "GHI9012",
            Modelo = "Uno",
            Tipo = 'A',
            Proprietario = "Vermelho"
        };
        _repositoryMock.Setup(x => x.BuscaVeiculoPorPlaca(veiculo.Placa)).ReturnsAsync((VeiculoModel)null);
        
        _repositoryMock.Setup(x => x.CadastraVeiculo(veiculos[0])).Returns(Task.CompletedTask);
        GenericResponse retornoEsperado = new() { Success = true, Message = "Veiculo cadastrado com sucesso", Data = null};

        // Act
        GenericResponse result = await _veiculoBusiness.CadastraVeiculo(veiculo);

        // Assert
        Assert.Equivalent(retornoEsperado, result);
    }

    [Fact]
    public async void CadastraVeiculo_SemSucesso_PlacaInvalida()
    {
        // Arrange
        CadastrarVeiculoDTO veiculo = new()
        {
            Placa = "GHI901",
            Modelo = "Uno",
            Tipo = 'A',
            Proprietario = "Vermelho"
        };
        GenericResponse retornoEsperado = new() { Success = false, Message = "Placa invalida", Data = null};

        // Act
        GenericResponse result = await _veiculoBusiness.CadastraVeiculo(veiculo);

        // Assert
        Assert.Equivalent(retornoEsperado, result);
    }

    [Fact]
    public async void CadastraVeiculo_SemSucesso_VeiculoJaCadastrado()
    {
        // Arrange
        CadastrarVeiculoDTO veiculo = new()
        {
            Placa = "ABC1234",
            Modelo = "Fusca",
            Tipo = 'A',
            Proprietario = "Branco"
        };
        _repositoryMock.Setup(x => x.BuscaVeiculoPorPlaca(veiculo.Placa)).ReturnsAsync(veiculos[0]);
        GenericResponse retornoEsperado = new() { Success = false, Message = "Veiculo já cadastrado", Data = null};

        // Act
        GenericResponse result = await _veiculoBusiness.CadastraVeiculo(veiculo);

        // Assert
        Assert.Equivalent(retornoEsperado, result);
    }

    [Fact]
    public async void AtualizarVeiculo_ComSucesso()
    {
        // Arrange
        AtualizarVeiculoDTO veiculo = new()
        {
            VeiculoID = 1,
            Placa = "ABC1234",
            Modelo = "Fusca",
            Tipo = "2",
            Proprietario = "Branco"
        };
        _repositoryMock.Setup(x => x.BuscaVeiculoPorId(1)).ReturnsAsync(veiculos[0]);
        _repositoryMock.Setup(x => x.AtualizaVeiculo(veiculos[0])).Returns(Task.CompletedTask);
        GenericResponse retornoEsperado = new() { Success = true, Message = "Veiculo atualizado com sucesso", Data = null};

        // Act
        GenericResponse result = await _veiculoBusiness.AtualizaVeiculo(veiculo);

        // Assert
        Assert.Equivalent(retornoEsperado, result);
    }

    [Fact]
    public async void AtualizarVeiculo_SemSucesso_PlacaInvalida()
    {
        // Arrange
        AtualizarVeiculoDTO veiculo = new()
        {
            VeiculoID = 1,
            Placa = "ABC123",
            Modelo = "Fusca",
            Tipo = "2",
            Proprietario = "Branco"
        };
        GenericResponse retornoEsperado = new() { Success = false, Message = "Placa invalida", Data = null};

        // Act
        GenericResponse result = await _veiculoBusiness.AtualizaVeiculo(veiculo);

        // Assert
        Assert.Equivalent(retornoEsperado, result);
    }

    [Fact]
    public async void AtualizarVeiculo_SemSucesso_PlacaJaCadastrada()
    {
        // Arrange
        AtualizarVeiculoDTO veiculo = new()
        {
            VeiculoID = 1,
            Placa = "DEF5678",
            Modelo = "Fusca",
            Tipo = "2",
            Proprietario = "Branco"
        };
        _repositoryMock.Setup(x => x.BuscaVeiculoPorPlaca(veiculo.Placa)).ReturnsAsync(veiculos[1]);
        GenericResponse retornoEsperado = new() { Success = false, Message = "Placa já cadastrada", Data = null};

        // Act
        GenericResponse result = await _veiculoBusiness.AtualizaVeiculo(veiculo);

        // Assert
        Assert.Equivalent(retornoEsperado, result);
    }

    [Fact]
    public async void AtualizarVeiculo_SemSucesso_VeiculoNaoEncontrado()
    {
        // Arrange
        AtualizarVeiculoDTO veiculo = new()
        {
            VeiculoID = 3,
            Placa = "GHI9012",
            Modelo = "Uno",
            Tipo = "2",
            Proprietario = "Vermelho"
        };
        _repositoryMock.Setup(x => x.BuscaVeiculoPorId(3)).ReturnsAsync((VeiculoModel)null);
        GenericResponse retornoEsperado = new() { Success = false, Message = "Veiculo não encontrado", Data = null};

        // Act
        GenericResponse result = await _veiculoBusiness.AtualizaVeiculo(veiculo);

        // Assert
        Assert.Equivalent(retornoEsperado, result);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async void DeletaVeiculo_ComSucesso(int id)
    {
        // Arrange
        _repositoryMock.Setup(x => x.BuscaVeiculoPorId(id)).ReturnsAsync(veiculos[id - 1]);
        _repositoryMock.Setup(x => x.DeletaVeiculo(veiculos[id - 1])).Returns(Task.CompletedTask);
        GenericResponse retornoEsperado = new() { Success = true, Message = "Veiculo deletado com sucesso", Data = null};

        // Act
        GenericResponse result = await _veiculoBusiness.DeletaVeiculoPorId(id);

        // Assert
        Assert.Equivalent(retornoEsperado, result);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(4)]
    public async void DeletaVeiculo_SemSucesso(int id)
    {
        // Arrange
        _repositoryMock.Setup(x => x.BuscaVeiculoPorId(id)).ReturnsAsync((VeiculoModel)null);
        GenericResponse retornoEsperado = new() { Success = false, Message = "Veiculo não encontrado", Data = null};

        // Act
        GenericResponse result = await _veiculoBusiness.DeletaVeiculoPorId(id);

        // Assert
        Assert.Equivalent(retornoEsperado, result);
    }

    [Fact]
    public async void DeletaVeiculo_SemSucesso_VeiculoNaoEncontrado()
    {
        // Arrange
        _repositoryMock.Setup(x => x.BuscaVeiculoPorId(3)).ReturnsAsync((VeiculoModel)null);
        GenericResponse retornoEsperado = new() { Success = false, Message = "Veiculo não encontrado", Data = null};

        // Act
        GenericResponse result = await _veiculoBusiness.DeletaVeiculoPorId(3);

        // Assert
        Assert.Equivalent(retornoEsperado, result);
    }

    [Fact]
    public async void DeletaVeiculoPorPlaca_ComSucesso()
    {
        // Arrange
        _repositoryMock.Setup(x => x.BuscaVeiculoPorPlaca("ABC1234")).ReturnsAsync(veiculos[0]);
        _repositoryMock.Setup(x => x.DeletaVeiculo(veiculos[0])).Returns(Task.CompletedTask);
        GenericResponse retornoEsperado = new() { Success = true, Message = "Veiculo deletado com sucesso", Data = null};

        // Act
        GenericResponse result = await _veiculoBusiness.DeletaVeiculoPorPlaca("ABC1234");

        // Assert
        Assert.Equivalent(retornoEsperado, result);
    }
    
    [Fact]
    public async void DeletaVeiculoPorPlaca_SemSucesso_PlacaNaoEncontrada()
    {
        // Arrange
        _repositoryMock.Setup(x => x.BuscaVeiculoPorPlaca("GHI9012")).ReturnsAsync((VeiculoModel)null);
        GenericResponse retornoEsperado = new() { Success = false, Message = "Veiculo não encontrado", Data = null};

        // Act
        GenericResponse result = await _veiculoBusiness.DeletaVeiculoPorPlaca("GHI9012");

        // Assert
        Assert.Equivalent(retornoEsperado, result);
    }
}
