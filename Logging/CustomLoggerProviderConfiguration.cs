namespace PrimeiraAPI.Logging
{
    public class CustomLoggerProviderConfiguration
    {
        public LogLevel LogLevel { get; set; } // Define o nível de log a ser registrado por padrão o LogLevel.Warning.
        public int EventId { get; set; } // Define o Id do evento do log, com o padrão sendo o zero. 
    }
}
