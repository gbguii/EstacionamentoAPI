using EstacionamentoV2.Business;
using EstacionamentoV2.Business.DTO;
using EstacionamentoV2.Business.Interface;
using EstacionamentoV2.Model;
using EstacionamentoV2.Repository.Interface;

namespace EstacionamentoV2;

public class RegistroVeiculoBusiness: IRegistroVeiculoBusiness
{
    private readonly IRegistroVeiculoRepository _registroRepository;
    private readonly IVeiculoRepository _veiculoRepository;
    private readonly IPatioRepository _patioRepository;
    public RegistroVeiculoBusiness(IRegistroVeiculoRepository registroRepository, IVeiculoRepository veiculoRepository, IPatioRepository patioRepository)
    {
        _registroRepository = registroRepository;
        _veiculoRepository = veiculoRepository;
        _patioRepository = patioRepository;
    }
    public async Task<GenericResponse> ConsultarVeiculoEstacionado()
    {
        List<RegistroVeiculoModel> registros = await _registroRepository.ConsultarVeiculosEstacionados();
        if(registros.Count == 0)
        {
            return new GenericResponse{Success = false,  Message = "Nenhum veículo estacionado"};
        }
        return new GenericResponse{Success = true,  Data = registros};
    }

    public async Task<GenericResponse> RegistrarEntradaVeiculo(RegistrarEntradaVeiculoDTO veiculo)
    {
        if(!Validacoes.ValidaPlacaVeiculo(veiculo.Placa))
        {
            return new GenericResponse{Success = false,  Message = "Placa inválida"};
        }
        
        PatioModel patio = await _patioRepository.RecuperaPatioPorId(veiculo.PatioId);

        if(patio == null)
        {
            return new GenericResponse{Success = false,  Message = "Pátio inválido"};
        }

        VeiculoModel veiculoExistente = await _veiculoRepository.BuscaVeiculoPorPlaca(veiculo.Placa);
        RegistroVeiculoModel registroVeiculo = new()
        {
            Placa = veiculo.Placa,
            VeiculoId = veiculoExistente?.VeiculoID,
            PatioID = veiculo.PatioId,
            Modelo = veiculo.Modelo,
            Mensalista = veiculo.Mensalista,
            DataEntrada = DateTime.Now
        };
        await _registroRepository.RegistrarEntradaVeiculo (registroVeiculo);
        return new GenericResponse{Success = true,  Message = "Veículo registrado com sucesso"};
    }

    public async Task<GenericResponse> RegistrarSaidaVeiculo(RegistrarSaidaVeiculoDTO veiculo)
    {
        if(!Validacoes.ValidaPlacaVeiculo(veiculo.Placa))
        {
            return new GenericResponse{Success = false,  Message = "Placa inválida"};
        }

        RegistroVeiculoModel registroVeiculo = await _registroRepository.ConsultarVeiculoEstacionado(veiculo.Placa);
        if(registroVeiculo == null)
        {
            return new GenericResponse{Success = false,  Message = "Veículo não encontrado"};
        }

        if(registroVeiculo.DataSaida != null)
        {
            return new GenericResponse{Success = false,  Message = "Veículo já liberado"};
        }

        registroVeiculo.DataSaida = DateTime.Now;
        int valorAPagar = CalcularValorPagar(registroVeiculo);
        await _registroRepository.RegistrarSaidaVeiculo(registroVeiculo);

        if(registroVeiculo.Mensalista)
        {
            return new GenericResponse{Success = true,  Message = "Veículo Mensalista liberado"};
        }
        
        
        if(valorAPagar == 0)
        {
            return new GenericResponse{Success = true,  Message = "Veículo liberado"};
        }
        
        return new GenericResponse{Success = true,  Message = $"Valor a pagar: {valorAPagar}"};
    }

    private static int CalcularValorPagar(RegistroVeiculoModel veiculo)
    {
        int valorPagar = 0;
        TimeSpan tempoEstacionado = veiculo.DataSaida.Value - veiculo.DataEntrada;
        if (veiculo.Mensalista)
        {
           valorPagar = 0;
        }
        else if(tempoEstacionado.TotalMinutes <= 30)
        {
            valorPagar = 5;
        }
        else if(tempoEstacionado.TotalHours <= 1)
        {
            valorPagar = 10;
        }
        else if(tempoEstacionado.TotalHours <= 2)
        {
            valorPagar = 12;
        }
        else if(tempoEstacionado.TotalHours > 2 && tempoEstacionado.TotalHours <= 6)
        {
            valorPagar = 15;
        }
        else if (tempoEstacionado.TotalHours > 6 && tempoEstacionado.TotalHours <= 14)
        {
            valorPagar = 18;
        }
        else
        {
            int TotalHoras = (int) tempoEstacionado.TotalHours - 14;
            valorPagar = 18 + (TotalHoras * 2);
        }
        veiculo.ValorPago = valorPagar;
        return valorPagar;
    }
}
