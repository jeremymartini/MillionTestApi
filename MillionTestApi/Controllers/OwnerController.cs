using MongoDB.Bson;
using MongoDB.Driver;

using Microsoft.AspNetCore.Mvc;

using MillionTestApi.DTOS;

namespace MillionTestApi.Controllers
{
    /// <summary>
    /// Controlador para gestionar las operaciones relacionadas con los propietarios (owners).
    /// </summary>
    /// <param name="logger">El servicio de registro para la clase.</param>
    /// <param name="database">La instancia de la base de datos MongoDB.</param>
    [ApiController]
    [Route("[controller]")]
    public class OwnerController(ILogger<OwnerController> logger, IMongoDatabase database) : ControllerBase
    {
        private readonly ILogger<OwnerController> _logger = logger;
        private readonly IMongoCollection<BsonDocument> _ownerCollection = database.GetCollection<BsonDocument>("owner");

        /// <summary>
        /// Obtiene un propietario por su ID.
        /// </summary>
        /// <param name="id">El ID del propietario a buscar. Debe ser un ObjectId válido.</param>
        /// <returns>
        /// Un <see cref="IActionResult"/> que contiene el propietario si se encuentra, 
        /// o un código de estado 404 si no existe.
        /// </returns>
        /// <response code="200">Retorna el propietario solicitado.</response>
        /// <response code="404">Si no se encuentra el propietario con el ID proporcionado.</response>
        /// <response code="500">Si ocurre un error interno en el servidor.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                // Convierte el ID de string a un ObjectId para la consulta de MongoDB.
                var objectId = new ObjectId(id);

                // Define la etapa de la pipeline de agregación para buscar por _id.
                var pipeline = new BsonDocument[]
                {
                    new("$match", new BsonDocument("_id", objectId))
                };

                // Ejecuta la pipeline de agregación y mapea el resultado a un DTO (Data Transfer Object).
                var result = await _ownerCollection.Aggregate<OwnerDto>(pipeline).FirstOrDefaultAsync();

                // Si el resultado es nulo, significa que no se encontró el propietario.
                if (result == null)
                {
                    return NotFound();
                }

                // Si se encuentra el propietario, lo retorna con un código de estado 200 (OK).
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Captura cualquier excepción, la registra y retorna un error 500.
                _logger.LogError(ex, "Ocurrió un error al recuperar el propietario con ID {0}.", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
