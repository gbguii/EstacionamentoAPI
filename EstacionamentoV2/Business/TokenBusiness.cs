using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EstacionamentoV2.Business.DTO;
using EstacionamentoV2.Business.Interface;
using EstacionamentoV2.Model;
using EstacionamentoV2.Repository.Interface;
using Microsoft.IdentityModel.Tokens;

namespace EstacionamentoV2.Business;

public class TokenBusiness : ITokenBusiness
{
    private readonly IUsuarioRepository _usuarioRepository;
    public TokenBusiness(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<GenericResponse> GerarToken(UsuarioTokenDTO usuario)
    {
        if (await ValidaUsuario(usuario))
        {
            return await GenereteToken(usuario);
        }
        return new GenericResponse() { Success = false, Message = "Usuário não encontrado ou inativo." };
    }

    private async Task<GenericResponse> GenereteToken(UsuarioTokenDTO usuario)
    {
        UsuarioModel usuarioModel = await RetornaUsuario(usuario);

        byte[] key = Encoding.ASCII.GetBytes(Key.Secret);
        SecurityTokenDescriptor tokenConfig = new()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Name, "EstacionamentoV2"),
                new(ClaimTypes.Role, usuarioModel.Acesso)
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = tokenHandler.CreateToken(tokenConfig);
        string tokenString = tokenHandler.WriteToken(token);
        return new GenericResponse() { Success = true, Message = "Token gerado com sucesso.", Data = tokenString };
    }

    private async Task<bool> ValidaUsuario(UsuarioTokenDTO usuarioDTO)
    {
        UsuarioModel usuario = await RetornaUsuario(usuarioDTO);
        return usuario != null && usuario.Ativo;
    }

    private async Task<UsuarioModel> RetornaUsuario(UsuarioTokenDTO usuarioTokenDTO)
    {
        string senhaCriptografada = CriptografiaSenhaBusiness.CriptografarSenha(usuarioTokenDTO.Senha);
        return await _usuarioRepository.RetornaUsuario(usuarioTokenDTO.Login, senhaCriptografada);
    }
}
