using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MillionTestApi.DTOS
{
    /// <summary>
    /// Proporciona una base para los objetos de transferencia de datos (DTOs)
    /// que representan documentos de MongoDB, incluyendo un campo de ID único.
    /// </summary>
    public class BaseDto
    {
        /// <summary>
        /// El identificador único del documento en MongoDB.
        /// Se mapea al campo '_id' de la colección.
        /// </summary>
        [BsonId]
        [BsonElement("_id")]
        public ObjectId Id { get; set; }
    }
}
