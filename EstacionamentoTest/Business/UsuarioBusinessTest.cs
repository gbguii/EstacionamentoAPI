using EstacionamentoV2.Business;
using EstacionamentoV2.Business.DTO;
using EstacionamentoV2.Model;
using EstacionamentoV2.Repository.Interface;
using Moq;

namespace EstacionamentoTest.Repository;

public class UsuarioBusinessTest
{
    private UsuarioBusiness _usuarioBusiness;
    private readonly Mock<IUsuarioRepository> _repositoryMock;

    public UsuarioBusinessTest()
    {
        _repositoryMock = new Mock<IUsuarioRepository>();
        _usuarioBusiness = new UsuarioBusiness(_repositoryMock.Object);
    }

    private readonly UsuarioModel usuarioModel = new() 
    {   Id = 1, 
        Login = "teste", 
        Senha = "teste", 
        Acesso = "teste", 
        DataCriacao = DateTime.Now, 
        DataAlteracao = DateTime.Now, 
        Ativo = true 
    };
    private readonly RegistrarUsuarioDTO usuarioDTO = new() 
    {   Login = "teste", 
        Senha = "teste", 
        Acesso = "teste" 
    };

    [Fact]
    public async void RetornaUsuario_ComSucesso()
    {
        // Arrange
        _repositoryMock.Setup(x => x.RetornaUsuario("teste", "teste")).ReturnsAsync(usuarioModel);
        GenericResponse retornoEsperado = new() { Success = true, Message = "Usuário retornado com sucesso.", Data = usuarioModel };

        // Act
        GenericResponse retorno = await _usuarioBusiness.RetornaUsuario("teste", "teste");

        // Assert
        Assert.Equivalent(retorno, retornoEsperado);
    }

    [Fact]
    public async void RetornaUsuario_SemSucesso_UsuarioNaoEncontrado()
    {
        // Arrange
        _repositoryMock.Setup(x => x.RetornaUsuario("teste", "teste")).ReturnsAsync((UsuarioModel)null);
        GenericResponse retornoEsperado = new() { Success = false, Message = "Usuário não encontrado.", Data = null };

        // Act
        GenericResponse retorno = await _usuarioBusiness.RetornaUsuario("teste", "teste");

        // Assert
        Assert.Equivalent(retorno, retornoEsperado);
    }

    [Fact]
    public async void RetornaUsuarioPorLogin_ComSucesso()
    {
        // Arrange
        _repositoryMock.Setup(x => x.RetornaUsuarioPorLogin("teste")).ReturnsAsync(usuarioModel);
        GenericResponse retornoEsperado = new() { Success = true, Message = "Usuário retornado com sucesso.", Data = usuarioModel };

        // Act
        GenericResponse retorno = await _usuarioBusiness.RetornaUsuarioPorLogin("teste");

        // Assert
        Assert.Equivalent(retorno, retornoEsperado);
    }

    [Fact]
    public async void RetornaUsuarioPorLogin_SemSucesso_UsuarioNaoEncontrado()
    {
        // Arrange
        _repositoryMock.Setup(x => x.RetornaUsuarioPorLogin("teste")).ReturnsAsync((UsuarioModel)null);
        GenericResponse retornoEsperado = new() { Success = false, Message = "Usuário não encontrado.", Data = null };

        // Act
        GenericResponse retorno = await _usuarioBusiness.RetornaUsuarioPorLogin("teste");

        // Assert
        Assert.Equivalent(retorno, retornoEsperado);
    }

    [Fact]
    public async void CriaUsuario_ComSucesso()
    {
        // Arrange
        _repositoryMock.Setup(x => x.RetornaUsuarioPorLogin("teste")).ReturnsAsync((UsuarioModel)null);
        _repositoryMock.Setup(x => x.CriaUsuario(It.IsAny<UsuarioModel>()));
        GenericResponse retornoEsperado = new() { Success = true, Message = "Usuário criado com sucesso.", Data = null };

        // Act
        GenericResponse retorno = await _usuarioBusiness.CriaUsuario(usuarioDTO);

        // Assert
        Assert.Equivalent(retorno, retornoEsperado);
    }

    [Fact]
    public async void CriaUsuario_SemSucesso_LoginJaExistente()
    {
        // Arrange
        _repositoryMock.Setup(x => x.RetornaUsuarioPorLogin("teste")).ReturnsAsync(usuarioModel);
        GenericResponse retornoEsperado = new() { Success = false, Message = "Login já existente.", Data = null };

        // Act
        GenericResponse retorno = await _usuarioBusiness.CriaUsuario(usuarioDTO);

        // Assert
        Assert.Equivalent(retorno, retornoEsperado);
    }

}
