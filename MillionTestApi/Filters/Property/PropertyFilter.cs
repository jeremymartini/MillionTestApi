using System.Text.Json.Serialization;

using MongoDB.Bson.Serialization.Attributes;

namespace MillionTestApi.Filters.Property
{
    /// <summary>
    /// Representa una estructura de filtro para buscar propiedades en la base de datos.
    /// Las propiedades de esta clase se utilizan para construir la consulta de agregación de MongoDB.
    /// </summary>
    public class PropertyFilter
    {
        /// <summary>
        /// Filtro de campo para el nombre de la propiedad.
        /// Permite búsquedas de texto usando expresiones regulares.
        /// </summary>
        [BsonIgnoreIfNull]
        [BsonElement("Name"), JsonPropertyName("Name")]
        public FieldFilter? Name { get; set; }

        /// <summary>
        /// Filtro de campo para la dirección de la propiedad.
        /// Permite búsquedas de texto usando expresiones regulares.
        /// </summary>
        [BsonIgnoreIfNull]
        [BsonElement("Address"), JsonPropertyName("Address")]
        public FieldFilter? Address { get; set; }

        /// <summary>
        /// Filtro de rango de precios para la propiedad.
        /// Permite buscar propiedades dentro de un rango de precios mínimo y máximo.
        /// </summary>
        [BsonIgnoreIfNull]
        [BsonElement("Price"), JsonPropertyName("Price")]
        public Price? Price { get; set; }
    }

    /// <summary>
    /// Representa un filtro de campo genérico que utiliza expresiones regulares para la búsqueda.
    /// Esta clase mapea directamente a los operadores de consulta de MongoDB.
    /// </summary>
    public class FieldFilter
    {
        /// <summary>
        /// Obtiene o establece el patrón de expresión regular a buscar.
        /// Se mapea al operador de MongoDB "$regex".
        /// </summary>
        [BsonElement("$regex"), JsonPropertyName("$regex")]
        public string? Regex { get; set; }

        /// <summary>
        /// Obtiene o establece las opciones para la búsqueda de expresión regular.
        /// Por defecto es "i" para hacer la búsqueda insensible a mayúsculas y minúsculas.
        /// Se mapea al operador de MongoDB "$options".
        /// </summary>
        [BsonElement("$options"), JsonPropertyName("$options")]
        public string? Options { get; set; } = "i";
    }

    /// <summary>
    /// Representa un filtro de rango de precios.
    /// Esta clase se utiliza para definir un rango de precios mínimo y máximo.
    /// </summary>
    public class Price
    {
        /// <summary>
        /// Obtiene o establece el precio mínimo (mayor o igual que).
        /// Se mapea al operador de MongoDB "$gte".
        /// </summary>
        [BsonIgnoreIfNull]
        [BsonElement("$gte"), JsonPropertyName("$gte")]
        public int? Min { get; set; }

        /// <summary>
        /// Obtiene o establece el precio máximo (menor o igual que).
        /// Se mapea al operador de MongoDB "$lte".
        /// </summary>
        [BsonIgnoreIfNull]
        [BsonElement("$lte"), JsonPropertyName("$lte")]
        public int? Max { get; set; }
    }
}
