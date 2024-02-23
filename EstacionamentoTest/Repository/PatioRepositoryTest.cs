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
    }

    private async Task AdicionaPatio()
    {
        // Arrange
        var patio = new PatioModel
        {
            PatioID = 1,
            PatioNome = "Patio 1",
            PatioVagas = 10,
        };
        List<PatioModel> patios = await _repository.RecuperaPatios();
        if(patios.Count > 0)
        {
            foreach (var p in patios)
            {
                await _repository.DeletaPatio(p);
            }
        }
        await _repository.CadastraPatio(patio);
        await _context.SaveChangesAsync();
    }  
    
    [Fact]
    public async void BuscaTodosPatios_ComSucesso()
    {
        // Arrange
        await AdicionaPatio();

        // Act
        List<PatioModel> patios = await _repository.RecuperaPatios();

        // Assert
        Assert.NotNull(patios);
    } 

    [Fact]
    public async void BuscaPatioPorId_ComSucesso()
    {
        // Arrange
        await AdicionaPatio();
        // Act
        PatioModel patio = await _repository.RecuperaPatioPorId(1);

        // Assert
        Assert.NotNull(patio);
    }

    [Fact]
    public async void AtualizaPatio_ComSucesso()
    {
        // Arrange
        await AdicionaPatio();
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
        await AdicionaPatio();
        PatioModel patio = await _repository.RecuperaPatioPorId(1);

        // Act
        await _repository.DeletaPatio(patio);
        PatioModel patioDeletado = await _repository.RecuperaPatioPorId(1);

        // Assert
        Assert.Null(patioDeletado);
    }
}
