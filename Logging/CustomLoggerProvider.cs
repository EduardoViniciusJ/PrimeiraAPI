using System.Collections.Concurrent;

namespace PrimeiraAPI.Logging
{
    // Implementa o ILoggerProvider, que é responsável por fornecer instâncias de ILogger.
    public class CustomLoggerProvider : ILoggerProvider
    {
        // Configuração do logger recebida por injeção de dependência.
        readonly CustomLoggerProviderConfiguration loggerConfig;

        // Um ConcurrentDictionary armazena os loggers para cada categoria de log. 
        // A chave é o nome da categoria (string) e o valor é o logger correspondente (CustomLogger).
        readonly ConcurrentDictionary<string, CustomLogger> loggers = new ConcurrentDictionary<string, CustomLogger>();

        // Construtor que inicializa o loggerConfig com as configurações fornecidas.
        public CustomLoggerProvider(CustomLoggerProviderConfiguration config)
        {
            loggerConfig = config;
        }

        // Cria ou retorna um logger para uma categoria específica.
        // Se o logger já existir para a categoria, ele o retorna. Caso contrário, cria um novo.
        public ILogger CreateLogger(string categoryName)
        {
            // O GetOrAdd tenta obter um logger para a categoria, e se não encontrar, cria um novo.
            return loggers.GetOrAdd(categoryName, name => new CustomLogger(name, loggerConfig));
        }

        // Limpa todos os loggers armazenados no ConcurrentDictionary quando o provider for descartado.
        public void Dispose()
        {
            loggers.Clear();
        }
    }
}
