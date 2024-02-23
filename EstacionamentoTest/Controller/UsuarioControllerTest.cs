using EstacionamentoV2.Business.DTO;
using EstacionamentoV2.Business.Interface;
using EstacionamentoV2.Controller;
using EstacionamentoV2.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EstacionamentoTest.Controllers;

public class UsuarioControllerTest
{

    private readonly Mock<IUsuarioBusiness> _usuarioBusinessMock;
    private readonly UsuarioController _usuarioController;

    public UsuarioControllerTest()
    {
        _usuarioBusinessMock = new Mock<IUsuarioBusiness>();
        _usuarioController = new UsuarioController(_usuarioBusinessMock.Object);
    }

    [Fact]
    public async Task RetornaUsuario_ComSucesso()
    {
        // Arrange
        string login = "login";
        string senha = "senha";
        UsuarioModel usuario = new UsuarioModel { Id = 1, Login = login, Senha = senha };
        _usuarioBusinessMock.Setup(x => x.RetornaUsuario(login, senha)).ReturnsAsync(new GenericResponse { Success = true, Message = "Usuário encontrado com sucesso.", Data = usuario });

        // Act
        IActionResult result = await _usuarioController.RetornaUsuario(login, senha);

        // Assert
        Assert.Multiple(() => 
        {
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<UsuarioModel>(okResult.Value);
            Assert.Equal(usuario, okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
        });
    }

    [Fact]
    public async void RetornaUsuario_SemSucesso_LoginInvalido()
    {
        // Arrange
        string login = "";
        string senha = "senha";

        // Act
        IActionResult result = await _usuarioController.RetornaUsuario(login, senha);

        // Assert
        Assert.Multiple(() => 
        {
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Login ou senha invalidos.", badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        });
        
    }

    [Fact]
    public async void RetornaUsuario_SemSucesso_SenhaInvalida()
    {
        // Arrange
        string login = "login";
        string senha = "";

        // Act
        IActionResult result = await _usuarioController.RetornaUsuario(login, senha);

        // Assert
        Assert.Multiple(() => 
        {
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Login ou senha invalidos.", badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        });
    }

    [Fact]
    public async void RetornaUsuario_SemSucesso_UsuarioNaoEncontrado()
    {
        // Arrange
        string login = "login";
        string senha = "senha";
        _usuarioBusinessMock.Setup(x => x.RetornaUsuario(login, senha)).ReturnsAsync(new GenericResponse { Success = false, Message = "Usuário não encontrado." });

        // Act
        IActionResult result = await _usuarioController.RetornaUsuario(login, senha);

        // Assert
        Assert.Multiple(() => 
        {
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Usuário não encontrado.", badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        });
    }

    [Fact]
    public async void RetornaUsuarioPorLogin_ComSucesso()
    {
        // Arrange
        string login = "login";
        UsuarioModel usuario = new UsuarioModel { Id = 1, Login = login, Senha = "senha" };
        _usuarioBusinessMock.Setup(x => x.RetornaUsuarioPorLogin(login)).ReturnsAsync(new GenericResponse { Success = true, Message = "Usuário encontrado com sucesso.", Data = usuario });

        // Act
        IActionResult result = await _usuarioController.RetornaUsuarioPorLogin(login);

        // Assert
        Assert.Multiple(() => 
        {
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<UsuarioModel>(okResult.Value);
            Assert.Equal(usuario, okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
        });
    }

    [Fact]
    public async void RetornaUsuarioPorLogin_SemSucesso_LoginInvalido()
    {
        // Arrange
        string login = "";
        _usuarioBusinessMock.Setup(x => x.RetornaUsuarioPorLogin(login)).ReturnsAsync(new GenericResponse { Success = false, Message = "Login invalido." });

        // Act
        IActionResult result = await _usuarioController.RetornaUsuarioPorLogin(login);

        // Assert
        Assert.Multiple(() => 
        {
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Login invalido.", badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        });
    }

    [Fact]
    public void RetornaUsuarioPorLogin_SemSucesso_UsuarioNaoEncontrado()
    {
        // Arrange
        string login = "login";
        _usuarioBusinessMock.Setup(x => x.RetornaUsuarioPorLogin(login)).ReturnsAsync(new GenericResponse { Success = false, Message = "Usuário não encontrado." });

        // Act
        IActionResult result = _usuarioController.RetornaUsuarioPorLogin(login).Result;

        // Assert
        Assert.Multiple(() => 
        {
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Usuário não encontrado.", badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        });
    }

    [Fact]
    public async void CadastrarUsuario_ComSucesso()
    {
        // Arrange
        RegistrarUsuarioDTO usuario = new RegistrarUsuarioDTO { Login = "login", Senha = "senha", Acesso = "acesso" };
        _usuarioBusinessMock.Setup(x => x.CriaUsuario(usuario)).ReturnsAsync(new GenericResponse { Success = true, Message = "Usuário cadastrado com sucesso." });

        // Act
        IActionResult result = await _usuarioController.CadastrarUsuario(usuario);

        // Assert
        Assert.Multiple(() => 
        {
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal("Usuário cadastrado com sucesso.", okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
        });
    }

    [Fact]
    public async void CadastrarUsuario_SemSucesso_CorpoRequisicaoInvalido()
    {
        // Arrange
        RegistrarUsuarioDTO usuario = null;

        // Act
        IActionResult result = await _usuarioController.CadastrarUsuario(usuario);

        // Assert
        Assert.Multiple(() => 
        {
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Corpo da requisição invalido.", badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        });
    }

    [Fact]
    public async void CadastrarUsuario_SemSucesso_LoginInvalido()
    {
        // Arrange
        RegistrarUsuarioDTO usuario = new RegistrarUsuarioDTO { Login = "lo", Senha = "senha", Acesso = "acesso" };

        // Act
        IActionResult result = await _usuarioController.CadastrarUsuario(usuario);

        // Assert
        Assert.Multiple(() => 
        {
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Login invalido.", badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        });
    }

    [Fact]
    public async void CadastrarUsuario_SemSucesso_SenhaInvalida()
    {
        // Arrange
        RegistrarUsuarioDTO usuario = new RegistrarUsuarioDTO { Login = "login", Senha = "sen", Acesso = "acesso" };

        // Act
        IActionResult result = await _usuarioController.CadastrarUsuario(usuario);

        // Assert
        Assert.Multiple(() => 
        {
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Senha invalida.", badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        });
    }

    [Fact]
    public async void CadastrarUsuario_SemSucesso_AcessoInvalido()
    {
        // Arrange
        RegistrarUsuarioDTO usuario = new RegistrarUsuarioDTO { Login = "login", Senha = "senha", Acesso = "a" };

        // Act
        IActionResult result = await _usuarioController.CadastrarUsuario(usuario);

        // Assert
        Assert.Multiple(() => 
        {
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Acesso invalido.", badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        });
    }

    [Fact]
    public async void CadastrarUsuario_SemSucesso_LoginJaCadastrado()
    {
        // Arrange
        RegistrarUsuarioDTO usuario = new RegistrarUsuarioDTO { Login = "login", Senha = "senha", Acesso = "acesso" };
        _usuarioBusinessMock.Setup(x => x.CriaUsuario(usuario)).ReturnsAsync(new GenericResponse { Success = false, Message = "Login já cadastrado." });

        // Act
        IActionResult result = await _usuarioController.CadastrarUsuario(usuario);

        // Assert
        Assert.Multiple(() => 
        {
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Login já cadastrado.", badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        });
    }

}
    
