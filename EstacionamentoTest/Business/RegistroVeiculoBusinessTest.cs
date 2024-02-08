using EstacionamentoV2;
using EstacionamentoV2.Business.DTO;
using EstacionamentoV2.Model;
using EstacionamentoV2.Repository.Interface;
using Moq;

namespace EstacionamentoTest.Business;

public class RegistroVeiculoBusinessTest
{
    private readonly RegistroVeiculoBusiness _registroVeiculoBusiness;
    private readonly Mock<IRegistroVeiculoRepository> _repositoryMock;
    private readonly Mock<IVeiculoRepository> _veiculoRepositoryMock;
    private readonly Mock<IPatioRepository> _patioRepositoryMock;
    public RegistroVeiculoBusinessTest()
    {
        _repositoryMock = new();
        _veiculoRepositoryMock = new();
        _patioRepositoryMock = new();
        _registroVeiculoBusiness = new(_repositoryMock.Object, _veiculoRepositoryMock.Object, _patioRepositoryMock.Object);
    }

    private readonly List<RegistroVeiculoModel> VeiculosEstacionado = new()
    {
        new RegistroVeiculoModel
        {
            Id = 1,
            Placa = "ABC1234",
            VeiculoId = 1,
            PatioID = 1,
            Modelo = "Fusca",
            Mensalista = false,
            DataEntrada = DateTime.Now
        }
    };
    private readonly RegistrarEntradaVeiculoDTO RegistroVeiculoDto = new()
    {
            Placa = "ABC1234",
            PatioId = 1,
            Modelo = "Fusca",
            Mensalista = false
    };

    private readonly PatioModel Patio = new()
    {
        PatioID = 1,
        PatioNome = "Patio 1",
        PatioVagas = 10,
        DataCadastro = DateTime.Now,
        DataAtualizacao = DateTime.Now
    };

    private readonly VeiculoModel Veiculo = new()
    {
        VeiculoID = 1,
        VeiculoPlaca = "ABC1234",
        VeiculoModelo = "Fusca",
        VeiculoTipo = '1',
        DataCriacao = DateTime.Now,
        DataAtualizacao = DateTime.Now
    };

    private RegistrarSaidaVeiculoDTO RegistroSaidaVeiculo = new()
    {
        Placa = "ABC1234",
        Ticket = 1
    };

    [Fact]
    public async void ConsultarVeiculoEstacionado_ComSucesso()
    {
        // Arrange
        _repositoryMock.Setup(x => x.ConsultarVeiculosEstacionados()).ReturnsAsync(VeiculosEstacionado);
        GenericResponse retornoEsperado = new() { Success = true, Data = VeiculosEstacionado };
        // Act
        GenericResponse retorno = await _registroVeiculoBusiness.ConsultarVeiculoEstacionado();
        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }

    [Fact]
    public async void ConsultarVeiculoEstacionado_SemVeiculosEstacionados()
    {
        // Arrange
        _repositoryMock.Setup(x => x.ConsultarVeiculosEstacionados()).ReturnsAsync(new List<RegistroVeiculoModel>());
        GenericResponse retornoEsperado = new() { Success = false, Message = "Nenhum veículo estacionado" };
        // Act
        GenericResponse retorno = await _registroVeiculoBusiness.ConsultarVeiculoEstacionado();
        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }

    [Fact]
    public async void RegistrarEntradaVeiculo_ComSucesso()
    {
        // Arrange
        RegistrarEntradaVeiculoDTO veiculo = new()
        {
            Placa = "ABC1234",
            PatioId = 1,
            Modelo = "Fusca",
            Mensalista = false
        };
        _patioRepositoryMock.Setup(x => x.RecuperaPatioPorId(RegistroVeiculoDto.PatioId)).ReturnsAsync(Patio);
        _veiculoRepositoryMock.Setup(x => x.BuscaVeiculoPorPlaca(RegistroVeiculoDto.Placa)).ReturnsAsync(Veiculo);
        _repositoryMock.Setup(x => x.RegistrarEntradaVeiculo(It.IsAny<RegistroVeiculoModel>()));
        GenericResponse retornoEsperado = new() { Success = true, Message = "Veículo registrado com sucesso" };
        // Act
        GenericResponse retorno = await _registroVeiculoBusiness.RegistrarEntradaVeiculo(veiculo);
        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }

    [Fact]
    public async void RegistrarEntradaVeiculo_PatioInvalido()
    {
        // Arrange
        _patioRepositoryMock.Setup(x => x.RecuperaPatioPorId(RegistroVeiculoDto.PatioId)).ReturnsAsync((PatioModel)null);
        GenericResponse retornoEsperado = new() { Success = false, Message = "Pátio inválido" };
        // Act
        GenericResponse retorno = await _registroVeiculoBusiness.RegistrarEntradaVeiculo(RegistroVeiculoDto);
        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }

    [Fact]
    public async void RegistrarEntradaVeiculo_PlacaInvalida()
    {
        // Arrange
        RegistrarEntradaVeiculoDTO veiculo = new()
        {
            Placa = "ABC12A3",
            PatioId = 1,
            Modelo = "Fusca",
            Mensalista = false
        };
        _patioRepositoryMock.Setup(x => x.RecuperaPatioPorId(RegistroVeiculoDto.PatioId)).ReturnsAsync(Patio);
        GenericResponse retornoEsperado = new() { Success = false, Message = "Placa inválida" };
        // Act
        GenericResponse retorno = await _registroVeiculoBusiness.RegistrarEntradaVeiculo(veiculo);
        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }

    [Fact]
    public async void RegistrarSaidaVeiculo_ComSucesso()
    {
        // Arrange
        RegistroVeiculoModel veiculo = VeiculosEstacionado[0];
        veiculo.DataEntrada = DateTime.Now.AddMinutes(-20);
        _repositoryMock.Setup(x => x.ConsultarVeiculoEstacionado(RegistroVeiculoDto.Placa)).ReturnsAsync(veiculo);
        _repositoryMock.Setup(x => x.RegistrarSaidaVeiculo(It.IsAny<RegistroVeiculoModel>()));
        GenericResponse retornoEsperado = new() { Success = true, Message = $"Valor a pagar: {5}"};
        // Act
        GenericResponse retorno = await _registroVeiculoBusiness.RegistrarSaidaVeiculo(RegistroSaidaVeiculo);
        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }

    [Fact]
    public async void RegistraSaidaVeiculo_ComSucesso_TotalPermanenciaGratis()
    {
        // Arrange
        RegistroVeiculoModel veiculo = VeiculosEstacionado[0];
        _repositoryMock.Setup(x => x.ConsultarVeiculoEstacionado(RegistroVeiculoDto.Placa)).ReturnsAsync(veiculo);
        _repositoryMock.Setup(x => x.RegistrarSaidaVeiculo(It.IsAny<RegistroVeiculoModel>()));
        GenericResponse retornoEsperado = new() { Success = true, Message = $"Veículo liberado"};
        // Act
        GenericResponse retorno = await _registroVeiculoBusiness.RegistrarSaidaVeiculo(RegistroSaidaVeiculo);
        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }
    

    [Fact]
    public async void RegistrarSaidaVeiculo_PlacaVazia()
    {
        // Arrange
        RegistrarSaidaVeiculoDTO veiculo = new()
        {
            Placa = "",
            Ticket = 1
        };
        GenericResponse retornoEsperado = new() { Success = false, Message = "Placa inválida" };
        // Act
        GenericResponse retorno = await _registroVeiculoBusiness.RegistrarSaidaVeiculo(veiculo);
        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }

    public async void RegistrarSaidaVeiculo_PlacaMenorQuantidae()
    {
        // Arrange
        RegistrarSaidaVeiculoDTO veiculo = new()
        {
            Placa = "ABC123",
            Ticket = 1
        };
        GenericResponse retornoEsperado = new() { Success = false, Message = "Placa inválida" };
        // Act
        GenericResponse retorno = await _registroVeiculoBusiness.RegistrarSaidaVeiculo(veiculo);
        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }

    public async void RegistrarSaidaVeiculo_PlacaMaiorQuantidae()
    {
        // Arrange
        RegistrarSaidaVeiculoDTO veiculo = new()
        {
            Placa = "ABC12345",
            Ticket = 1
        };
        GenericResponse retornoEsperado = new() { Success = false, Message = "Placa inválida" };
        // Act
        GenericResponse retorno = await _registroVeiculoBusiness.RegistrarSaidaVeiculo(veiculo);
        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }

    [Fact]
    public async void RegistrarSaidaVeiculo_VeiculoNaoEncontrado()
    {
        // Arrange
        _repositoryMock.Setup(x => x.ConsultarVeiculoEstacionado(RegistroVeiculoDto.Placa)).ReturnsAsync((RegistroVeiculoModel)null);
        GenericResponse retornoEsperado = new() { Success = false, Message = "Veículo não encontrado" };
        // Act
        GenericResponse retorno = await _registroVeiculoBusiness.RegistrarSaidaVeiculo(RegistroSaidaVeiculo);
        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }

    [Fact]
    public async void RegistrarSaidaVeiculo_PlacaFormatoInvalido()
    {
        // Arrange
        RegistrarSaidaVeiculoDTO veiculo = new()
        {
            Placa = "ABC1BA3",
            Ticket = 1
        };
        GenericResponse retornoEsperado = new() { Success = false, Message = "Placa inválida" };
        // Act
        GenericResponse retorno = await _registroVeiculoBusiness.RegistrarSaidaVeiculo(veiculo);
        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }

    [Fact]
    public async void RegistrarSaidaVeiculo_VeiculoJaLiberado()
    {
        // Arrange
        RegistroVeiculoModel veiculo = VeiculosEstacionado[0];
        veiculo.DataSaida = DateTime.Now;
        _repositoryMock.Setup(x => x.ConsultarVeiculoEstacionado(RegistroVeiculoDto.Placa)).ReturnsAsync(veiculo);
        GenericResponse retornoEsperado = new() { Success = false, Message = "Veículo já liberado" };
        // Act
        GenericResponse retorno = await _registroVeiculoBusiness.RegistrarSaidaVeiculo(RegistroSaidaVeiculo);
        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }

    [Fact]
    public async void RegistrarSaidaVeiculo_VeiculoMensalista()
    {
        // Arrange
        RegistroVeiculoModel veiculo = VeiculosEstacionado[0];
        veiculo.Mensalista = true;
        _repositoryMock.Setup(x => x.ConsultarVeiculoEstacionado(RegistroVeiculoDto.Placa)).ReturnsAsync(veiculo);
        GenericResponse retornoEsperado = new() { Success = true, Message = "Veículo Mensalista liberado" };
        // Act
        GenericResponse retorno = await _registroVeiculoBusiness.RegistrarSaidaVeiculo(RegistroSaidaVeiculo);
        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }

    [Fact]
    public async void RegistrarSaidaVeiculo_1HoraPermanencia()
    {
        // Arrange
        RegistroVeiculoModel veiculo = VeiculosEstacionado[0];
        veiculo.DataEntrada = DateTime.Now.AddMinutes(-31);
        _repositoryMock.Setup(x => x.ConsultarVeiculoEstacionado(RegistroVeiculoDto.Placa)).ReturnsAsync(veiculo);
        GenericResponse retornoEsperado = new() { Success = true, Message = $"Valor a pagar: {10}" };
        // Act
        GenericResponse retorno = await _registroVeiculoBusiness.RegistrarSaidaVeiculo(RegistroSaidaVeiculo);
        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }

    [Fact]
    public async void RegistrarSaidaVeiculo_2HoraPermanencia()
    {
        // Arrange
        RegistroVeiculoModel veiculo = VeiculosEstacionado[0];
        veiculo.DataEntrada = DateTime.Now.AddMinutes(-65);
        _repositoryMock.Setup(x => x.ConsultarVeiculoEstacionado(RegistroVeiculoDto.Placa)).ReturnsAsync(veiculo);
        GenericResponse retornoEsperado = new() { Success = true, Message = $"Valor a pagar: {12}" };
        // Act
        GenericResponse retorno = await _registroVeiculoBusiness.RegistrarSaidaVeiculo(RegistroSaidaVeiculo);
        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }

    [Fact]
    public async void RegistrarSaidaVeiculo_3a6HorasPermanencia()
    {
        // Arrange
        RegistroVeiculoModel veiculo = VeiculosEstacionado[0];
        veiculo.DataEntrada = DateTime.Now.AddHours(-3);
        _repositoryMock.Setup(x => x.ConsultarVeiculoEstacionado(RegistroVeiculoDto.Placa)).ReturnsAsync(veiculo);
        GenericResponse retornoEsperado = new() { Success = true, Message = $"Valor a pagar: {15}" };
        // Act
        GenericResponse retorno = await _registroVeiculoBusiness.RegistrarSaidaVeiculo(RegistroSaidaVeiculo);
        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }

    [Fact]
    public async void RegistrarSaidaVeiculo_7a14HorasPermanencia()
    {
        // Arrange
        RegistroVeiculoModel veiculo = VeiculosEstacionado[0];
        veiculo.DataEntrada = DateTime.Now.AddHours(-7);
        _repositoryMock.Setup(x => x.ConsultarVeiculoEstacionado(RegistroVeiculoDto.Placa)).ReturnsAsync(veiculo);
        GenericResponse retornoEsperado = new() { Success = true, Message = $"Valor a pagar: {18}" };
        // Act
        GenericResponse retorno = await _registroVeiculoBusiness.RegistrarSaidaVeiculo(RegistroSaidaVeiculo);
        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }

    [Fact]
    public async void RegistrarSaidaVeiculo_Mais14horasPermanencia()
    {
        // Arrange
        RegistroVeiculoModel veiculo = VeiculosEstacionado[0];
        veiculo.DataEntrada = DateTime.Now.AddHours(-15);
        veiculo.DataSaida = DateTime.Now;
        TimeSpan tempoEstacionado = veiculo.DataSaida.Value - veiculo.DataEntrada;
        int TotalHoras = (int) tempoEstacionado.TotalHours - 14;
        int valorPagar = 18 + (TotalHoras * 2);
        veiculo.DataSaida = null;
        _repositoryMock.Setup(x => x.ConsultarVeiculoEstacionado(RegistroVeiculoDto.Placa)).ReturnsAsync(veiculo);
        GenericResponse retornoEsperado = new() { Success = true, Message = $"Valor a pagar: {valorPagar}" };
        // Act
        GenericResponse retorno = await _registroVeiculoBusiness.RegistrarSaidaVeiculo(RegistroSaidaVeiculo);
        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }

    [Fact]
    public async void RegistrarSaidaVeiculo_JaLiberado()
    {
        // Arrange
        RegistroVeiculoModel veiculo = VeiculosEstacionado[0];
        veiculo.DataSaida = DateTime.Now;
        _repositoryMock.Setup(x => x.ConsultarVeiculoEstacionado(RegistroVeiculoDto.Placa)).ReturnsAsync(veiculo);
        GenericResponse retornoEsperado = new() { Success = false, Message = "Veículo já liberado" };
        // Act
        GenericResponse retorno = await _registroVeiculoBusiness.RegistrarSaidaVeiculo(RegistroSaidaVeiculo);
        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }


}
