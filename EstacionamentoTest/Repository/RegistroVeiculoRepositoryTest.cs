using EstacionamentoV2.Context;
using EstacionamentoV2.Model;
using EstacionamentoV2.Repository;

namespace EstacionamentoTest.Repository;

public class RegistroVeiculoRepositoryTest
{
    private readonly EstacionamentoContext _context;
    private readonly RegistroVeiculoRepository _repository;

    public RegistroVeiculoRepositoryTest()
    {
        _context = GeradorContextoEmMemoria.GerarContexto();
        _repository = new RegistroVeiculoRepository(_context);
    }

    private async void AdicionaVeiculoEstacionado()
    {
        // Arrange
        var registro = new RegistroVeiculoModel
        {
            Placa = "ABC1234",
            DataEntrada = DateTime.Now,
            Modelo = "Gol",
            Mensalista = false,
            PatioID = 1,
        };
        await _repository.RegistrarEntradaVeiculo(registro);
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async void BuscaTodosVeiculosEstacionados_ComSucesso()
    {
        // Arrange
        AdicionaVeiculoEstacionado();

        // Act
        List<RegistroVeiculoModel> veiculosEstacionados = await _repository.ConsultarVeiculosEstacionados();

        // Assert
        Assert.NotNull(veiculosEstacionados);
    }

    [Fact]
    public async void BuscaVeiculoPorPlaca_ComSucesso()
    {
        // Arrange
        AdicionaVeiculoEstacionado();

        // Act
        RegistroVeiculoModel veiculo = await _repository.ConsultarVeiculoEstacionado("ABC1234");

        // Assert
        Assert.Equal("ABC1234", veiculo.Placa);
    }

    [Fact]
    public async void RegistraEntradaVeiculo_ComSucesso()
    {
        // Arrange
        var registro = new RegistroVeiculoModel
        {
            Placa = "ABC1234",
            DataEntrada = DateTime.Now,
            Modelo = "Gol",
            Mensalista = false,
            PatioID = 1,
        };

        // Act
        await _repository.RegistrarEntradaVeiculo(registro);

        // Assert
        RegistroVeiculoModel veiculo = await _repository.ConsultarVeiculoEstacionado("ABC1234");
        Assert.NotNull(veiculo);
    }

    [Fact]
    public async void RegistraSaidaVeiculo_ComSucesso()
    {
        // Arrange
        AdicionaVeiculoEstacionado();
        RegistroVeiculoModel veiculo = await _repository.ConsultarVeiculoEstacionado("ABC1234");
        veiculo.DataSaida = DateTime.Now;

        // Act
        await _repository.RegistrarSaidaVeiculo(veiculo);

        // Assert
        RegistroVeiculoModel veiculoSaida = await _repository.ConsultarVeiculoEstacionado("ABC1234");
        Assert.NotNull(veiculoSaida.DataSaida);
    }
}
