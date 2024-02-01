using EstacionamentoV2.Business;
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
    public async Task RecuperaPatioPorId_ComSuceso()
    {
        _repositoryMock.Setup(x => x.RecuperaPatioPorId(1)).Returns(Task.FromResult(new PatioModel
        {
            PatioID = 1,
            PatioNome = "Patio 1",
            PatioVagas = 10,
            DataCadastro = DateTime.Now,
            DataAtualizacao = DateTime.Now
        }));

        var patio = await _patioBusiness.RecuperaPatioPorId(1);
        Assert.NotNull(patio);
    }

    [Fact]
    public async Task RecuperaPatioPorId_SemSuceso()
    {
        _repositoryMock.Setup(x => x.RecuperaPatioPorId(2)).Returns(Task.FromResult<PatioModel>(null));
        var patio = await _patioBusiness.RecuperaPatioPorId(2);
        Assert.Null(patio.Data);
    }
}
