using EstacionamentoV2.Context;
using EstacionamentoV2.Model;
using EstacionamentoV2.Repository;

namespace EstacionamentoTest.Repository;

public class VeiculoRepoitoryTest
{
    private readonly EstacionamentoContext _context;
    private readonly VeiculoRepository _repository;

    public VeiculoRepoitoryTest()
    {
        _context = GeradorContextoEmMemoria.GerarContexto();
        _repository = new VeiculoRepository(_context);
    }

    private async Task AdicionaVeiculo()
    {
        var veiculo = new VeiculoModel
        {
            VeiculoID = 1,
            VeiculoPlaca = "ABC1234",
            VeiculoModelo = "Uno",
            VeiculoTipo = '1',
            VeiculoProprietario = "João",
            DataAtualizacao = DateTime.Now,
            DataCriacao = DateTime.Now
        };

        List<VeiculoModel> veiculos = await _repository.BuscaTodosVeiculos();
        foreach (var v in veiculos)
        {
            await _repository.DeletaVeiculo(v);
        }
        await _repository.CadastraVeiculo(veiculo);
    }

    [Fact]
    public async Task BuscaTodosVeiculos_ComSucesso_DeveRetornarListaDeVeiculos()
    {
        // Arrange
        await AdicionaVeiculo();

        // Act
        var veiculos = await _repository.BuscaTodosVeiculos();
        
        // Assert
        Assert.NotEmpty(veiculos);
    }

    [Fact]
    public async Task BuscaVeiculoPorId_ComSuceso_DeveRetornarVeiculo()
    {
        // Arrange
        await AdicionaVeiculo();

        // Act
        var veiculo = await _repository.BuscaVeiculoPorId(1);

        // Assert
        Assert.NotNull(veiculo);
    }

    [Fact]
    public async Task BuscaVeiculoPorPlaca_ComSucesso()
    {
        // Arrange
        await AdicionaVeiculo();

        // Act
        var veiculo = await _repository.BuscaVeiculoPorPlaca("ABC1234");

        // Assert
        Assert.Equal("ABC1234", veiculo.VeiculoPlaca);
    }

    [Fact]
    public async Task CadastraVeiculo_ComSucesso()
    {
        // Arrange
        var veiculo = new VeiculoModel
        {
            VeiculoPlaca = "AVT0132",
            VeiculoModelo = "Uno",
            VeiculoTipo = '1',
            VeiculoProprietario = "João",
            DataAtualizacao = DateTime.Now,
            DataCriacao = DateTime.Now
        };

        // Act
        await _repository.CadastraVeiculo(veiculo);
        var veiculoCadastrado = await _repository.BuscaVeiculoPorPlaca("AVT0132");

        // Assert
        Assert.Equal(veiculoCadastrado.VeiculoModelo, veiculo.VeiculoModelo);
    }

    [Fact]
    public async Task AtualizaVeiculo_ComSucesso()
    {
        // Arrange
        await AdicionaVeiculo();
        var veiculo = await _repository.BuscaVeiculoPorId(1);
        veiculo.VeiculoModelo = "Gol";

        // Act
        await _repository.AtualizaVeiculo(veiculo);
        var veiculoAtualizado = await _repository.BuscaVeiculoPorId(1);

        // Assert
        Assert.Equal("Gol", veiculoAtualizado.VeiculoModelo);
    }

    [Fact]
    public async Task DeletaVeiculo_ComSucesso()
    {
        // Arrange
        await AdicionaVeiculo();
        var veiculo = await _repository.BuscaVeiculoPorId(1);

        // Act
        await _repository.DeletaVeiculo(veiculo);
        var veiculoDeletado = await _repository.BuscaVeiculoPorId(1);

        // Assert
        Assert.Null(veiculoDeletado);
    }
}
