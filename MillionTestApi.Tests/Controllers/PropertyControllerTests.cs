using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using MongoDB.Bson;
using MongoDB.Driver;

using Moq;

using MillionTestApi.Controllers;

namespace MillionTestApi.Tests.Controllers
{
    [TestFixture]
    public class PropertyControllerTests
    {
        private Mock<ILogger<OwnerController>> _loggerMock = null!;
        private PropertyController _controller = null!;

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
            _controller = new PropertyController(_loggerMock.Object, database);
        }

        [Test]
        public async Task GetPaginatedByFilter_ReturnsOkResult_WithJson()
        {
            // Act
            var result = await _controller.GetPaginatedByFilter(1, 10, null);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.Not.Null);
        }

        [Test]
        public async Task GetPaginatedByFilter_ReturnsInternalServerError_OnException()
        {
            // Act
            var result = await _controller.GetPaginatedByFilter(-1);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var objectResult = (ObjectResult)result;
            Assert.That(objectResult.StatusCode, Is.EqualTo(500));
            Assert.That(objectResult.Value, Is.EqualTo("Error interno del servidor"));
        }

        [Test]
        public async Task GetTraceByPropertyId_ReturnsOkResult_WhenTraceExists()
        {
            // Arrange
            var propertyId = "64c5d58d3f9b2c3a5e8b456a"; // Recuerden reemplazar por ids que existan en su base de datos

            // Act
            var result = await _controller.GetTraceByPropertyId(propertyId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.Not.Null);
        }

        [Test]
        public async Task GetTraceByPropertyId_ReturnsNotFound_WhenTraceIsNull()
        {
            // Arrange
            var propertyId = ObjectId.GenerateNewId().ToString();   

            // Act
            var result = await _controller.GetTraceByPropertyId(propertyId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetTraceByPropertyId_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var propertyId = "invalid_object_id";

            // Act
            var result = await _controller.GetTraceByPropertyId(propertyId);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var objectResult = (ObjectResult)result;
            Assert.That(objectResult.StatusCode, Is.EqualTo(500));
            Assert.That(objectResult.Value, Is.EqualTo("Error interno del servidor"));
        }
    }
}
