using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.IO;

using Microsoft.AspNetCore.Mvc;


using MillionTestApi.DTOS;
using MillionTestApi.Filters.Property;

namespace MillionTestApi.Controllers
{
    /// <summary>
    /// Controlador para gestionar las operaciones relacionadas con las propiedades (properties).
    /// </summary>
    /// <param name="logger">El servicio de registro para la clase.</param>
    /// <param name="database">La instancia de la base de datos MongoDB.</param>
    [ApiController]
    [Route("[controller]")]
    public class PropertyController(ILogger<OwnerController> logger, IMongoDatabase database) : ControllerBase
    {
        private readonly ILogger<OwnerController> _logger = logger;
        private readonly IMongoCollection<BsonDocument> _propertyCollection = database.GetCollection<BsonDocument>("property");
        private readonly IMongoCollection<BsonDocument> _propertyTraceCollection = database.GetCollection<BsonDocument>("property_trace");

        /// <summary>
        /// Obtiene una lista paginada de propiedades, opcionalmente filtrada.
        /// </summary>
        /// <param name="page">El número de página a recuperar (por defecto es 1).</param>
        /// <param name="pageSize">El tamaño de la página (número de documentos por página, por defecto es 10).</param>
        /// <param name="filter">Un objeto de filtro para aplicar a las propiedades.</param>
        /// <returns>
        /// Un <see cref="IActionResult"/> que contiene la lista de propiedades paginadas y filtradas.
        /// </returns>
        /// <response code="200">Retorna la lista de propiedades.</response>
        /// <response code="500">Si ocurre un error interno en el servidor.</response>
        [HttpPost("page/{page:int}/pageSize/{pageSize:int}")]
        public async Task<IActionResult> GetPaginatedByFilter(int page = 1, int pageSize = 10, [FromBody] PropertyFilter? filter = null)
        {
            try
            {
                // Calcula cuántos documentos se deben omitir para la paginación.
                var skip = (page - 1) * pageSize;
                var pipeline = new List<BsonDocument>();

                // Agrega una etapa $match a la pipeline si se proporciona un filtro.
                if (filter != null 
                    && (filter.Name is not null 
                    || filter.Address is not null 
                    || filter.Price is not null))
                {
                    pipeline.Add(new BsonDocument("$match", filter.ToBsonDocument()));
                }

                // Agrega las etapas de paginación ($skip y $limit) a la pipeline.
                pipeline.Add(new BsonDocument("$skip", skip));
                pipeline.Add(new BsonDocument("$limit", pageSize));

                // Realiza un $lookup para unir las imágenes de las propiedades.
                pipeline.Add(new BsonDocument("$lookup",
                    new BsonDocument
                    {
                    { "from", "property_image" },
                    { "localField", "_id" },
                    { "foreignField", "IdProperty" },
                    { "as", "image" }
                    }));

                // Ejecuta la pipeline de agregación y retorna los resultados.
                var result = await _propertyCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();

                return Ok(result.ToJson(new JsonWriterSettings { Indent = true }));
            }
            catch (Exception ex)
            {
                // Captura cualquier excepción, la registra y retorna un error 500.
                _logger.LogError(ex, "Ocurrió un error al recuperar las propiedades.");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene el historial (trace) de una propiedad por su ID.
        /// </summary>
        /// <param name="idProperty">El ID de la propiedad para la que se busca el historial.</param>
        /// <returns>
        /// Un <see cref="IActionResult"/> que contiene el historial de la propiedad si se encuentra, 
        /// o un código de estado 404 si no existe un historial para el ID proporcionado.
        /// </returns>
        /// <response code="200">Retorna el historial de la propiedad solicitado.</response>
        /// <response code="404">Si no se encuentra el historial para el ID de propiedad proporcionado.</response>
        /// <response code="500">Si ocurre un error interno en el servidor.</response>
        [HttpGet("trace/{idProperty}")]
        public async Task<IActionResult> GetTraceByPropertyId(string idProperty)
        {
            try
            {
                // Convierte el ID de string a un ObjectId para la consulta de MongoDB.
                var objectId = new ObjectId(idProperty);

                // Define la etapa de la pipeline de agregación para buscar por IdProperty.
                var pipeline = new BsonDocument[]
                {
                new("$match", new BsonDocument("IdProperty", objectId))
                };

                // Ejecuta la pipeline y retorna los resultados.
                var result = await _propertyTraceCollection.Aggregate<PropertyTraceDto>(pipeline).ToListAsync();

                // Si el resultado es nulo, significa que no se encontró el historial.
                if (result == null || !result.Any())
                {
                    return NotFound();
                }

                // Si se encuentra el historial, lo retorna con un código de estado 200 (OK).
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Captura cualquier excepción, la registra y retorna un error 500.
                _logger.LogError(ex, "Ocurrió un error al recuperar el historial de la propiedad con IdProperty {0}.", idProperty);
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
