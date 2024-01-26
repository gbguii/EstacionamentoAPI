using EstacionamentoV2.Business.DTO;
using EstacionamentoV2.Business.Interface;
using EstacionamentoV2.Model;
using EstacionamentoV2.Repository.Interface;

namespace EstacionamentoV2.Business;

public class VeiculoBusiness: IVeiculoBusiness
{
    private readonly IVeiculoRepository _veiculoRepository;
    public VeiculoBusiness(IVeiculoRepository veiculoRepository)
    {
        _veiculoRepository = veiculoRepository;
    }

    public async Task<GenericResponse> BuscaTodosVeiculos()
    {
        List<VeiculoModel> veiculos = await _veiculoRepository.BuscaTodosVeiculos();
        if (veiculos.Count == 0)
        {
            return new GenericResponse { Success = false, Message = "Nenhum veiculo encontrado" };
        }
        return new GenericResponse { Data = veiculos, Success = true, Message = "Veiculos encontrados com sucesso" };
    }

    public async Task<GenericResponse> BuscaVeiculoPorId(int id)
    {
        VeiculoModel veiculo = await _veiculoRepository.BuscaVeiculoPorId(id);
        if (veiculo == null)
        {
            return new GenericResponse { Success = false, Message = "Veiculo não encontrado" };
        }
        return new GenericResponse { Data = veiculo, Success = true, Message = "Veiculo encontrado com sucesso" };
    }

    public async Task<GenericResponse> BuscaVeiculoPorPlaca(string placa)
    {
        VeiculoModel veiculo = await _veiculoRepository.BuscaVeiculoPorPlaca(placa);
        if (veiculo == null)
        {
            return new GenericResponse { Success = false, Message = "Veiculo não encontrado" };
        }
        return new GenericResponse { Data = veiculo, Success = true, Message = "Veiculo encontrado com sucesso" };
    }

    public async Task<GenericResponse> CadastraVeiculo(CadastrarVeiculoDTO veiculo)
    {
        if (!Validacoes.ValidaPlacaVeiculo(veiculo.Placa))
        {
            return new GenericResponse { Success = false, Message = "Placa invalida" };
        }

        VeiculoModel veiculoExistente = await _veiculoRepository.BuscaVeiculoPorPlaca(veiculo.Placa);
        if (veiculoExistente != null)
        {
            return new GenericResponse { Success = false, Message = "Veiculo já cadastrado" };
        }

        VeiculoModel veiculoModel = new VeiculoModel
        {
            VeiculoPlaca = veiculo.Placa,
            VeiculoModelo = veiculo.Modelo,
            VeiculoTipo = veiculo.Tipo,
            VeiculoProprietario = veiculo.Proprietario,
            DataCriacao = DateTime.Now,
            DataAtualizacao = DateTime.Now,
        };
        await _veiculoRepository.CadastraVeiculo(veiculoModel);
        return new GenericResponse { Success = true, Message = "Veiculo cadastrado com sucesso" };
    }

    public async Task<GenericResponse> AtualizaVeiculo(AtualizarVeiculoDTO veiculo)
    {
        if (!Validacoes.ValidaPlacaVeiculo(veiculo.Placa))
        {
            return new GenericResponse { Success = false, Message = "Placa invalida" };
        }

        VeiculoModel RecuperaVeiculoPorPlaca = await _veiculoRepository.BuscaVeiculoPorPlaca(veiculo.Placa);
        if (RecuperaVeiculoPorPlaca != null && RecuperaVeiculoPorPlaca.VeiculoID != veiculo.VeiculoID)
        {
            return new GenericResponse { Success = false, Message = "Placa já cadastrada" };
        }

        VeiculoModel veiculoRecuperado = await _veiculoRepository.BuscaVeiculoPorId(veiculo.VeiculoID);
        if (veiculoRecuperado == null)
        {
            return new GenericResponse { Success = false, Message = "Veiculo não encontrado" };
        }

        veiculoRecuperado.VeiculoPlaca = veiculo.Placa;
        veiculoRecuperado.VeiculoModelo = veiculo.Modelo;
        veiculoRecuperado.VeiculoProprietario = veiculo.Proprietario;
        veiculoRecuperado.DataAtualizacao = DateTime.Now;
        await _veiculoRepository.AtualizaVeiculo(veiculoRecuperado);
        return new GenericResponse { Success = true, Message = "Veiculo atualizado com sucesso" };
    }

    public async Task<GenericResponse> DeletaVeiculoPorId(int id)
    {
        VeiculoModel veiculo = await _veiculoRepository.BuscaVeiculoPorId(id);
        if (veiculo == null)
        {
            return new GenericResponse { Success = false, Message = "Veiculo não encontrado" };
        }
        await _veiculoRepository.DeletaVeiculo(veiculo);
        return new GenericResponse { Success = true, Message = "Veiculo deletado com sucesso" };
    }

    public async Task<GenericResponse> DeletaVeiculoPorPlaca(string placa)
    {
        VeiculoModel veiculo = await _veiculoRepository.BuscaVeiculoPorPlaca(placa);
        if (veiculo == null)
        {
            return new GenericResponse { Success = false, Message = "Veiculo não encontrado" };
        }
        await _veiculoRepository.DeletaVeiculo(veiculo);
        return new GenericResponse { Success = true, Message = "Veiculo deletado com sucesso" };
    }
}
