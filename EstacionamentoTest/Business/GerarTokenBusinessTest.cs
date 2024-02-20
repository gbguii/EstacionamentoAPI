using EstacionamentoV2.Business;
using EstacionamentoV2.Business.DTO;
using EstacionamentoV2.Model;
using EstacionamentoV2.Repository.Interface;
using Moq;

namespace EstacionamentoTest.Business;

public class GerarTokenBusinessTest
{
    private readonly TokenBusiness _gerarTokenBusiness;
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;

    public GerarTokenBusinessTest()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _gerarTokenBusiness = new(_usuarioRepositoryMock.Object);
    }

    private readonly UsuarioModel _usuario = new()
    {
        Id = 1,
        Login = "usuario",
        Senha = "senha",
        Acesso = "Admin",
        Ativo = true
    };

    [Fact]
    public async void GerarToken_ComSucesso()
    {
        // Arrange
        var usuarioTokenDTO = new UsuarioTokenDTO
        {
            Login = "usuario",
            Senha = "senha"
        };
        _usuarioRepositoryMock.Setup(x => x.RetornaUsuario(usuarioTokenDTO.Login, usuarioTokenDTO.Senha)).ReturnsAsync(_usuario);

        // Act
        GenericResponse retorno = await _gerarTokenBusiness.GerarToken(usuarioTokenDTO);

        // Assert
        Assert.True(retorno.Success);
    }

    [Fact]
    public async void GerarToken_UsuarioAcesoCliente_ComSucesso()
    {
        // Arrange
        var usuarioTokenDTO = new UsuarioTokenDTO
        {
            Login = "usuario",
            Senha = "senha"
        };
        _usuario.Acesso = "Cliente";
        _usuarioRepositoryMock.Setup(x => x.RetornaUsuario(usuarioTokenDTO.Login, usuarioTokenDTO.Senha)).ReturnsAsync(_usuario);

        // Act
        GenericResponse retorno = await _gerarTokenBusiness.GerarToken(usuarioTokenDTO);

        // Assert
        Assert.True(retorno.Success);
    }

    [Fact]
    public async void GerarToken_Funcionario_ComSucesso()
    {
        // Arrange
        var usuarioTokenDTO = new UsuarioTokenDTO
        {
            Login = "usuario",
            Senha = "senha"
        };
        _usuario.Acesso = "Funcionario";
        _usuarioRepositoryMock.Setup(x => x.RetornaUsuario(usuarioTokenDTO.Login, usuarioTokenDTO.Senha)).ReturnsAsync(_usuario);

        // Act
        GenericResponse retorno = await _gerarTokenBusiness.GerarToken(usuarioTokenDTO);

        // Assert
        Assert.True(retorno.Success);
    }

    [Fact]
    public async void GerarToken_UsuarioNaoEncontrado()
    {
        // Arrange
        var usuarioTokenDTO = new UsuarioTokenDTO
        {
            Login = "usuario",
            Senha = "senha"
        };
        _usuarioRepositoryMock.Setup(x => x.RetornaUsuario(usuarioTokenDTO.Login, usuarioTokenDTO.Senha)).ReturnsAsync((UsuarioModel)null);

        // Act
        GenericResponse retorno = await _gerarTokenBusiness.GerarToken(usuarioTokenDTO);

        // Assert
        Assert.Equal("Usuário não encontrado ou inativo.", retorno.Message);
    }

    [Fact]
    public async void GerarToken_UsuarioInativo()
    {
        // Arrange
        var usuarioTokenDTO = new UsuarioTokenDTO
        {
            Login = "usuario",
            Senha = "senha"
        };
        _usuario.Ativo = false;
        _usuarioRepositoryMock.Setup(x => x.RetornaUsuario(usuarioTokenDTO.Login, usuarioTokenDTO.Senha)).ReturnsAsync(_usuario);

        // Act
        GenericResponse retorno = await _gerarTokenBusiness.GerarToken(usuarioTokenDTO);

        // Assert
        Assert.Equal("Usuário não encontrado ou inativo.", retorno.Message);
    }
}
