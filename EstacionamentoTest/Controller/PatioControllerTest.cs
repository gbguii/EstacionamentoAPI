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
        public async void RecuperaPatio_ComSucesso()
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
    }
}