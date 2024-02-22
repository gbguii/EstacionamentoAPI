using EstacionamentoV2.Business.DTO;
using EstacionamentoV2.Business.Interface;
using EstacionamentoV2.Controller;
using EstacionamentoV2.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;

namespace EstacionamentoTest.Controllers;

public class TokenControllerTest
{
    private readonly TokenController _tokenController;
    private readonly Mock<ITokenBusiness> _tokenBusinessMock;

    public TokenControllerTest()
    {
        _tokenBusinessMock = new Mock<ITokenBusiness>();
        _tokenController = new TokenController(_tokenBusinessMock.Object);
    }

    [Fact]
    public async void GerarToken_ComSucesso()
    {
        // Arrange
        UsuarioTokenDTO usuario = new(){Login = "admin", Senha = "admin"};
        GenericResponse retornoEsperado = new (){Success = true, Message = "Token gerado com sucesso.", Data = "token"};
        _tokenBusinessMock.Setup(x => x.GerarToken(usuario)).ReturnsAsync(
            retornoEsperado
        );


        // Act
        IActionResult retorno = await _tokenController.GerarToken(usuario);

        // Assert
        Assert.Multiple(() =>
        {
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(retorno);
            Assert.IsType<PatioModel>(okResult.Value);
            Assert.Equal(retornoEsperado.Data, okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
        });

    }

    [Fact]
    public async void GerarToken_UsuarioNaoExistente()
    {
        // Arrange
        UsuarioTokenDTO usuario = new(){Login = "admin", Senha = "10"};
        GenericResponse retornoEsperado = new (){Success = false, Message = "Usuario não encontrado ou inativo."};
        _tokenBusinessMock.Setup(x => x.GerarToken(usuario)).ReturnsAsync(
            retornoEsperado
        );

        // Act
        IActionResult retorno = await _tokenController.GerarToken(usuario);

        // Assert
        Assert.Multiple(() =>
        {
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(retorno);
            Assert.IsType<PatioModel>(badRequestResult.Value);
            Assert.Equal(retornoEsperado.Message, badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        });

    }
}
