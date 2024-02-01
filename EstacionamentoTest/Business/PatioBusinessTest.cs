using EstacionamentoV2.Business;
using EstacionamentoV2.Business.DTO;
using EstacionamentoV2.Model;
using EstacionamentoV2.Repository.Interface;
using Moq;

namespace EstacionamentoTest.Business;

public class PatioBusinessTest
{
    private readonly PatioBusiness _patioBusiness;
    private readonly Mock<IPatioRepository> _repositoryMock;
    public PatioBusinessTest()
    {
        _repositoryMock = new();
        _patioBusiness = new(_repositoryMock.Object);
    }

    [Fact]
    public async void RecuperaTodosPatios_ComSucesso()
    {
        // Arrange
        List<PatioModel> patios = new()
        {
            new PatioModel
            {
                PatioID = 1,
                PatioNome = "Patio 1",
                PatioVagas = 10,
                DataCadastro = DateTime.Now,
                DataAtualizacao = DateTime.Now
            },
            new PatioModel
            {
                PatioID = 2,
                PatioNome = "Patio 2",
                PatioVagas = 20,
                DataCadastro = DateTime.Now,
                DataAtualizacao = DateTime.Now
            }
        };
        _repositoryMock.Setup(x => x.RecuperaPatios()).Returns(Task.FromResult(patios));
        GenericResponse retornoEsperado = new() { Success = true, Message = "Patios recuperados com sucesso!", Data = patios};

        // Act
        GenericResponse retorno = await _patioBusiness.RecuperaPatios();

        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }

    [Fact]
    public async void RecuperaTodosPatios_SemSucesso()
    {
        // Arrange
        _repositoryMock.Setup(x => x.RecuperaPatios()).Returns(Task.FromResult(new List<PatioModel>()));
        GenericResponse retornoEsperado = new() { Success = false, Message = "Nenhum patio cadastrado!", Data = null};

        // Act
        GenericResponse patios = await _patioBusiness.RecuperaPatios();

        // Assert
        Assert.Equivalent(retornoEsperado, patios);
    }


    [Fact]
    public async void RecuperaPatioPorId_ComSuceso()
    {
        // Arrange
        PatioModel patio = new()
        {
            PatioID = 1,
            PatioNome = "Patio 1",
            PatioVagas = 10,
            DataCadastro = DateTime.Now,
            DataAtualizacao = DateTime.Now
        };
        _repositoryMock.Setup(x => x.RecuperaPatioPorId(1)).Returns(Task.FromResult(patio));
        GenericResponse retornoEsperado = new() { Success = true, Message = "Patio recuperado com sucesso!", Data = patio};

        // Act
        GenericResponse retorno = await _patioBusiness.RecuperaPatioPorId(1);

        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }

    [Fact]
    public async void RecuperaPatioPorId_SemSuceso()
    {
        // Arrange
        _repositoryMock.Setup(x => x.RecuperaPatioPorId(2)).Returns(Task.FromResult<PatioModel>(null));
        GenericResponse retornoEsperado = new() { Success = false, Message = "Nenhum patio encontrado!", Data = null};

        // Act
        GenericResponse patio = await _patioBusiness.RecuperaPatioPorId(2);

        // Assert
        Assert.Equivalent(retornoEsperado, patio);
    }

    [Fact]
    public async void AtualizaPatio_ComSucesso()
    {
        // Arrange
        AtualizarPatioDTO patio = new()
        {
            PatioID = 1,
            PatioNome = "Patio 1",
            PatioVagas = 10
        };
        PatioModel patioRecuperado = new()
        {
            PatioID = 1,
            PatioNome = "Patio 1",
            PatioVagas = 10,
            DataCadastro = DateTime.Now,
            DataAtualizacao = DateTime.Now
        };
        _repositoryMock.Setup(x => x.RecuperaPatioPorId(1)).Returns(Task.FromResult(patioRecuperado));
        _repositoryMock.Setup(x => x.AtualizaPatio(patioRecuperado)).Returns(Task.CompletedTask);
        GenericResponse retornoEsperado = new() { Success = true, Message = "Patio atualizado com sucesso!", Data = patio};

        // Act
        GenericResponse retorno = await _patioBusiness.AtualizaPatio(patio);

        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }

    [Fact]
    public async void AtualizaPatio_SemSucesso_PatioNaoEncontrado()
    {
        // Arrange
        AtualizarPatioDTO patio = new()
        {
            PatioID = 1,
            PatioNome = "Patio 1",
            PatioVagas = 10
        };
        _repositoryMock.Setup(x => x.RecuperaPatioPorId(1)).Returns(Task.FromResult<PatioModel>(null));
        GenericResponse retornoEsperado = new() { Success = false, Message = "Nenhum patio encontrado!", Data = null};

        // Act
        GenericResponse retorno = await _patioBusiness.AtualizaPatio(patio);

        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }

    [Fact]
    public async void CadastraPatio_ComSucesso()
    {
        // Arrange
        CadastrarPatioDTO patio = new()
        {
            PatioNome = "Patio 1",
            PatioVagas = 10
        };
        _repositoryMock.Setup(x => x.CadastraPatio(It.IsAny<PatioModel>())).Returns(Task.CompletedTask);
        GenericResponse retornoEsperado = new() { Success = true, Message = "Patio cadastrado com sucesso!", Data = patio};

        // Act
        GenericResponse retorno = await _patioBusiness.CadastraPatio(patio);

        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }

    [Fact]
    public async void DeletaPatio_ComSucesso()
    {
        // Arrange
        PatioModel patioRecuperado = new()
        {
            PatioID = 1,
            PatioNome = "Patio 1",
            PatioVagas = 10,
            DataCadastro = DateTime.Now,
            DataAtualizacao = DateTime.Now
        };
        _repositoryMock.Setup(x => x.RecuperaPatioPorId(1)).Returns(Task.FromResult(patioRecuperado));
        _repositoryMock.Setup(x => x.DeletaPatio(patioRecuperado)).Returns(Task.CompletedTask);
        GenericResponse retornoEsperado = new() { Success = true, Message = "Patio deletado com sucesso!", Data = null};

        // Act
        GenericResponse retorno = await _patioBusiness.DeletaPatio(1);

        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }

    [Fact]
    public async void DeletaPatio_SemSucesso_PatioNaoEncontrado()
    {
        // Arrange
        _repositoryMock.Setup(x => x.RecuperaPatioPorId(1)).Returns(Task.FromResult<PatioModel>(null));
        GenericResponse retornoEsperado = new() { Success = false, Message = "Nenhum patio encontrado!", Data = null};

        // Act
        GenericResponse retorno = await _patioBusiness.DeletaPatio(1);

        // Assert
        Assert.Equivalent(retornoEsperado, retorno);
    }
}
