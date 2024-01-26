using EstacionamentoV2.Business.DTO;
using EstacionamentoV2.Business.Interface;
using EstacionamentoV2.Model;
using EstacionamentoV2.Repository.Interface;

namespace EstacionamentoV2.Business;

public class PatioBusiness : IPatioBusiness
{
    private readonly IPatioRepository _patioRepository;
    public PatioBusiness(IPatioRepository patioRepository)
    {
        _patioRepository = patioRepository;
    }

    public async Task<GenericResponse> RecuperaPatios()
    {
        List<PatioModel> patios = await _patioRepository.RecuperaPatios();
        if(patios.Count == 0)
        {
            return new GenericResponse
            {
                Success = false,
                Message = "Nenhum patio cadastrado!",
                Data = null
            };
        }
        return new GenericResponse{Success = true,Message = "Patios recuperados com sucesso!", Data = patios};
    }

    public async Task<GenericResponse> RecuperaPatioPorId(int id)
    {
        PatioModel patio = await _patioRepository.RecuperaPatioPorId(id);
        
        if(patio == null)
        {
            return new GenericResponse
            {
                Success = false,
                Message = "Nenhum patio encontrado!",
                Data = null
            };
        }
        
        return new GenericResponse{Success = true,Message = "Patio recuperado com sucesso!", Data = patio};
    }

    public async Task<GenericResponse> AtualizaPatio(AtualizarPatioDTO patio)
    {
        PatioModel patioRecuperado = await _patioRepository.RecuperaPatioPorId(patio.PatioID);
        
        if(patioRecuperado == null)
        {
            return new GenericResponse {Success = false,Message = "Nenhum patio encontrado!",Data = null};
        }

        patioRecuperado.PatioNome = patio.PatioNome;
        patioRecuperado.PatioVagas = patio.PatioVagas;
        patioRecuperado.DataAtualizacao = DateTime.Now;

        await _patioRepository.AtualizaPatio(patioRecuperado);
        return new GenericResponse {Success = true,Message = "Patio atualizado com sucesso!",Data = patio};
    }

    public async Task<GenericResponse> CadastraPatio(CadastrarPatioDTO patio)
    {
        PatioModel patioModel = new PatioModel
        {
            PatioNome = patio.PatioNome,
            PatioVagas = patio.PatioVagas,
            DataCadastro = DateTime.Now,
            DataAtualizacao = DateTime.Now
        };
        await _patioRepository.CadastraPatio(patioModel);
        return new GenericResponse {Success = true,Message = "Patio cadastrado com sucesso!",Data = patio};
    }

    public async Task<GenericResponse> DeletaPatio(int id)
    {
        PatioModel patioRecuperado = await _patioRepository.RecuperaPatioPorId(id);
        
        if(patioRecuperado == null)
        {
            return new GenericResponse {Success = false,Message = "Nenhum patio encontrado!",Data = null};
        }

        await _patioRepository.DeletaPatio(patioRecuperado);
        return new GenericResponse {Success = true,Message = "Patio deletado com sucesso!",Data = null};
    }
}
