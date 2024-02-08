using EstacionamentoV2.Business.DTO;
using EstacionamentoV2.Business.Interface;
using EstacionamentoV2.Controller;
using EstacionamentoV2.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EstacionamentoTest.Controllers
{
    public class PatioControllerTest
    {
        private readonly PatioController _patioController;
        private readonly Mock<IPatioBusiness> _patioBusinessMock;

        public PatioControllerTest()
        {
            _patioBusinessMock = new Mock<IPatioBusiness>();
            _patioController = new PatioController(_patioBusinessMock.Object);
        }

        [Fact]
        public async void RecuperaPatios_ComSucesso()
        {
            // Arrange
            PatioModel patioEsperado = new();
            GenericResponse retornoEsperado = new (){ Success = true, Data = patioEsperado };
            _patioBusinessMock.Setup(x => x.RecuperaPatios()).ReturnsAsync(retornoEsperado);

            // Act
            IActionResult result = await _patioController.RecuperaPatios();

            // Assert
            Assert.Multiple(() =>
            {
                OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
                Assert.IsType<PatioModel>(okResult.Value);
                Assert.Equal(patioEsperado, okResult.Value);
                Assert.Equal(200, okResult.StatusCode);
            });
        }

        [Fact]
        public async void RecuperaPatios_SemSucesso_NenhumPatioEncontrado()
        {
            // Arrange
            GenericResponse retornoEsperado = new (){ Success = false, Message = "Nenhum patio encontrado!" };
            _patioBusinessMock.Setup(x => x.RecuperaPatios()).ReturnsAsync(retornoEsperado);

            // Act
            IActionResult result = await _patioController.RecuperaPatios();

            // Assert
            Assert.Multiple(() =>
            {
                BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<string>(badRequestResult.Value);
                Assert.Equal("Nenhum patio encontrado!", badRequestResult.Value);
                Assert.Equal(400, badRequestResult.StatusCode);
            });
        }

        [Fact]
        public async void RecuperaPatioPorId_ComSucesso()
        {
            // Arrange
            PatioModel patioEsperado = new();
            GenericResponse retornoEsperado = new (){ Success = true, Data = patioEsperado };
            _patioBusinessMock.Setup(x => x.RecuperaPatioPorId(It.IsAny<int>())).ReturnsAsync(retornoEsperado);

            // Act
            IActionResult result = await _patioController.RecuperaPatioPorId(1);

            // Assert
            Assert.Multiple(() =>
            {
                OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
                Assert.IsType<PatioModel>(okResult.Value);
                Assert.Equal(patioEsperado, okResult.Value);
                Assert.Equal(200, okResult.StatusCode);
            });
        }

        [Fact]
        public async void RecuperaPatioPorId_SemSucesso_IdInvalido()
        {
            // Arrange
            _patioBusinessMock.Setup(x => x.RecuperaPatioPorId(It.IsAny<int>())).ReturnsAsync(new GenericResponse { Success = false, Message = "Id invalido!" });

            // Act
            IActionResult result = await _patioController.RecuperaPatioPorId(0);

            // Assert
            Assert.Multiple(() =>
            {
                BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<string>(badRequestResult.Value);
                Assert.Equal("Id invalido!", badRequestResult.Value);
                Assert.Equal(400, badRequestResult.StatusCode);
            });
        }

        [Fact]
        public async void RecuperaPatioPorId_SemSucesso_NenhumPatioEncontrado()
        {
            // Arrange
            _patioBusinessMock.Setup(x => x.RecuperaPatioPorId(It.IsAny<int>())).ReturnsAsync(new GenericResponse { Success = false, Message = "Nenhum patio encontrado!" });

            // Act
            IActionResult result = await _patioController.RecuperaPatioPorId(1);

            // Assert
            Assert.Multiple(() =>
            {
                BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<string>(badRequestResult.Value);
                Assert.Equal("Nenhum patio encontrado!", badRequestResult.Value);
                Assert.Equal(400, badRequestResult.StatusCode);
            });
        }


        [Fact]
        public async void AtualizaPatio_ComSucesso()
        {
            // Arrange
            AtualizarPatioDTO patio = new(){ PatioID = 1, PatioNome = "Patio 1", PatioVagas = 10};
            GenericResponse retornoEsperado = new (){ Success = true, Message = "Patio atualizado com sucesso!"};
            _patioBusinessMock.Setup(x => x.AtualizaPatio(It.IsAny<AtualizarPatioDTO>())).ReturnsAsync(retornoEsperado);

            // Act
            IActionResult result = await _patioController.AtualizaPatio(patio);

            // Assert
            Assert.Multiple(() =>
            {
                OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
                Assert.IsType<string>(okResult.Value);
                Assert.Equal("Patio atualizado com sucesso!", okResult.Value);
                Assert.Equal(200, okResult.StatusCode);
            });
        }

        [Fact]
        public async void AtualizaPatio_SemSucesso_DadosInvalidos()
        {
            // Arrange
            _patioBusinessMock.Setup(x => x.AtualizaPatio(It.IsAny<AtualizarPatioDTO>())).ReturnsAsync(new GenericResponse { Success = false, Message = "Dados invalidos!" });

            // Act
            IActionResult result = await _patioController.AtualizaPatio(new AtualizarPatioDTO());

            // Assert
            Assert.Multiple(() =>
            {
                BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<string>(badRequestResult.Value);
                Assert.Equal("Dados invalidos!", badRequestResult.Value);
                Assert.Equal(400, badRequestResult.StatusCode);
            });
        }

        [Fact]
        public async void AtualizaPatio_SemSucesso_PatioNaoEncontrado()
        {
            // Arrange
            _patioBusinessMock.Setup(x => x.AtualizaPatio(It.IsAny<AtualizarPatioDTO>())).ReturnsAsync(new GenericResponse { Success = false, Message = "Patio nao encontrado!" });

            // Act
            IActionResult result = await _patioController.AtualizaPatio(new AtualizarPatioDTO { PatioID = 1, PatioNome = "Patio 1", PatioVagas = 10 });

            // Assert
            Assert.Multiple(() =>
            {
                BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<string>(badRequestResult.Value);
                Assert.Equal("Patio nao encontrado!", badRequestResult.Value);
                Assert.Equal(400, badRequestResult.StatusCode);
            });
        }

        [Fact]
        public async void AdicionaPatio_ComSucesso()
        {
            // Arrange
            CadastrarPatioDTO patio = new(){ PatioNome = "Patio 1", PatioVagas = 10};
            GenericResponse retornoEsperado = new (){ Success = true, Message = "Patio cadastrado com sucesso!"};
            _patioBusinessMock.Setup(x => x.CadastraPatio(It.IsAny<CadastrarPatioDTO>())).ReturnsAsync(retornoEsperado);

            // Act
            IActionResult result = await _patioController.AdicionaPatio(patio);

            // Assert
            Assert.Multiple(() =>
            {
                OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
                Assert.IsType<string>(okResult.Value);
                Assert.Equal("Patio cadastrado com sucesso!", okResult.Value);
                Assert.Equal(200, okResult.StatusCode);
            });
        }

        [Fact]
        public async void AdicionaPatio_SemSucesso_DadosInvalidos()
        {
            // Arrange
            _patioBusinessMock.Setup(x => x.CadastraPatio(It.IsAny<CadastrarPatioDTO>())).ReturnsAsync(new GenericResponse { Success = false, Message = "Dados invalidos!" });

            // Act
            IActionResult result = await _patioController.AdicionaPatio(new CadastrarPatioDTO());

            // Assert
            Assert.Multiple(() =>
            {
                BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<string>(badRequestResult.Value);
                Assert.Equal("Dados invalidos!", badRequestResult.Value);
                Assert.Equal(400, badRequestResult.StatusCode);
            });
        }

        [Fact]
        public async void AdicionaPatio_SemSucesso_PatioJaCadastrado()
        {
            // Arrange
            _patioBusinessMock.Setup(x => x.CadastraPatio(It.IsAny<CadastrarPatioDTO>())).ReturnsAsync(new GenericResponse { Success = false, Message = "Patio ja cadastrado!" });

            // Act
            IActionResult result = await _patioController.AdicionaPatio(new CadastrarPatioDTO { PatioNome = "Patio 1", PatioVagas = 10 });

            // Assert
            Assert.Multiple(() =>
            {
                BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<string>(badRequestResult.Value);
                Assert.Equal("Patio ja cadastrado!", badRequestResult.Value);
                Assert.Equal(400, badRequestResult.StatusCode);
            });
        }

        [Fact]
        public async void DeletaPatio_ComSucesso()
        {
            // Arrange
            GenericResponse retornoEsperado = new (){ Success = true, Message = "Patio deletado com sucesso!"};
            _patioBusinessMock.Setup(x => x.DeletaPatio(It.IsAny<int>())).ReturnsAsync(retornoEsperado);

            // Act
            IActionResult result = await _patioController.DeletaPatio(1);

            // Assert
            Assert.Multiple(() =>
            {
                OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
                Assert.IsType<string>(okResult.Value);
                Assert.Equal("Patio deletado com sucesso!", okResult.Value);
                Assert.Equal(200, okResult.StatusCode);
            });
        }

        [Fact]
        public async void DeletaPatio_SemSucesso_IdInvalido()
        {
            // Arrange
            _patioBusinessMock.Setup(x => x.DeletaPatio(It.IsAny<int>())).ReturnsAsync(new GenericResponse { Success = false, Message = "Id invalido!" });

            // Act
            IActionResult result = await _patioController.DeletaPatio(0);

            // Assert
            Assert.Multiple(() =>
            {
                BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<string>(badRequestResult.Value);
                Assert.Equal("Id invalido!", badRequestResult.Value);
                Assert.Equal(400, badRequestResult.StatusCode);
            });
        }

        [Fact]
        public async void DeletaPatio_SemSucesso_PatioNaoEncontrado()
        {
            // Arrange
            _patioBusinessMock.Setup(x => x.DeletaPatio(It.IsAny<int>())).ReturnsAsync(new GenericResponse { Success = false, Message = "Patio nao encontrado!" });

            // Act
            IActionResult result = await _patioController.DeletaPatio(1);

            // Assert
            Assert.Multiple(() =>
            {
                BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<string>(badRequestResult.Value);
                Assert.Equal("Patio nao encontrado!", badRequestResult.Value);
                Assert.Equal(400, badRequestResult.StatusCode);
            });
        }
    }
}