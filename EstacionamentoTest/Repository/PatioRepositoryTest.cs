using EstacionamentoV2.Context;
using EstacionamentoV2.Model;
using EstacionamentoV2.Repository;

namespace EstacionamentoTest.Repository;

public class PatioRepositoryTest
{
    private readonly EstacionamentoContext _context;
    private readonly PatioRepository _repository;

    public PatioRepositoryTest()
    {
        _context = GeradorContextoEmMemoria.GerarContexto();
        _repository = new PatioRepository(_context);
         AdicionaPatio();
    }

    private async void AdicionaPatio()
    {
        // Arrange
        var patio = new PatioModel
        {
            PatioNome = "Patio 1",
            PatioVagas = 10,
        };
        await _repository.CadastraPatio(patio);
        await _context.SaveChangesAsync();
    }  
    
    [Fact]
    public async void BuscaTodosPatios_ComSucesso()
    {
        // Act
        List<PatioModel> patios = await _repository.RecuperaPatios();

        // Assert
        Assert.NotNull(patios);
    } 

    [Fact]
    public async void BuscaPatioPorId_ComSucesso()
    {

        // Act
        PatioModel patio = await _repository.RecuperaPatioPorId(1);

        // Assert
        Assert.Equal("Patio 1", patio.PatioNome);
    }

    [Fact]
    public async void AtualizaPatio_ComSucesso()
    {
        PatioModel patio = await _repository.RecuperaPatioPorId(1);
        patio.PatioNome = "Patio 2";

        // Act
        await _repository.AtualizaPatio(patio);
        PatioModel patioAtualizado = await _repository.RecuperaPatioPorId(1);

        // Assert
        Assert.Equal("Patio 2", patioAtualizado.PatioNome);
    }

    [Fact]
    public async void CadastraPatio_ComSucesso()
    {
        // Arrange
        var patio = new PatioModel
        {
            PatioID = 10,
            PatioNome = "Patio 1",
            PatioVagas = 10,
        };

        // Act
        await _repository.CadastraPatio(patio);
        PatioModel patioCadastrado = await _repository.RecuperaPatioPorId(10);

        // Assert
        Assert.Equal("Patio 1", patioCadastrado.PatioNome);
    }

    [Fact]
    public async void DeletaPatio_ComSucesso()
    {
        // Arrange
        AdicionaPatio();
        PatioModel patio = await _repository.RecuperaPatioPorId(1);

        // Act
        await _repository.DeletaPatio(patio);
        PatioModel patioDeletado = await _repository.RecuperaPatioPorId(1);

        // Assert
        Assert.Null(patioDeletado);
    }
}
