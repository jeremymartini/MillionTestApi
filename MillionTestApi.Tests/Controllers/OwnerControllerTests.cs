using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using MongoDB.Bson;
using MongoDB.Driver;

using Moq;

using MillionTestApi.Controllers;

namespace MillionTestApi.Tests.Controllers
{
    [TestFixture]
    public class OwnerControllerTests
    {
        private Mock<ILogger<OwnerController>> _loggerMock = null!;
        private OwnerController _controller = null!;

        [SetUp]
        public void SetUp()
        {
            // Normalmente no necesitas esto en los tests unitarios
            // Se utilizan bases de datos de prueba con datos controlados
            // pero para efectos de demostracion y practicidad
            // Utilizaremos la misma
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("million_test_db");

            _loggerMock = new Mock<ILogger<OwnerController>>();
            _controller = new OwnerController(_loggerMock.Object, database);
        }

        // Reemplaza Assert.IsInstanceOf<T>(obj) por Assert.That(obj, Is.InstanceOf<T>())
        // Aplica esto en los tres métodos de prueba

        [Test]
        public async Task GetById_ReturnsOkResult_WhenOwnerExists()
        {
            // Arrange
            var ownerId = new ObjectId("64c5d57b3f9b2c3a5e8b4568"); // Recuerden reemplazar por ids que existan en su base de datos

            // Act
            var result = await _controller.GetById(ownerId.ToString());

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetById_ReturnsNotFound_WhenOwnerDoesNotExist()
        {
            // Arrange
            var ownerId = ObjectId.GenerateNewId().ToString();

            // Actd
            var result = await _controller.GetById(ownerId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetById_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var ownerId = "invalid_object_id"; // Provoca una excepción al crear ObjectId

            // Act
            var result = await _controller.GetById(ownerId);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var objectResult = (ObjectResult)result;
            Assert.That(objectResult.StatusCode, Is.EqualTo(500));
            Assert.That(objectResult.Value, Is.EqualTo("Error interno del servidor"));
        }
    }
}