namespace MillionTestApi.Settings
{
    /// <summary>
    /// Representa la configuración de conexión para la base de datos MongoDB.
    /// </summary>
    public class MongoDbSettings
    {
        /// <summary>
        /// Obtiene o establece la cadena de conexión completa para MongoDB.
        /// Esta cadena incluye la dirección del servidor, el puerto y las credenciales de autenticación si son necesarias.
        /// </summary>
        public required string ConnectionString { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de la base de datos a la que se desea conectar.
        /// </summary>
        public required string DatabaseName { get; set; }
    }
}
