namespace PrimeiraAPI.Logging
{
    // Classe CustomLogger implementa a interface ILogger. Este é o logger personalizado para registrar logs em um arquivo.
    public class CustomLogger : ILogger
    {
        readonly string loggerName; // Nome do logger.
        readonly CustomLoggerProviderConfiguration loggerConfig; // Configurações do logger.

        // Construtor que inicializa o nome do logger e a configuração.
        public CustomLogger(string name, CustomLoggerProviderConfiguration config)
        {
            loggerName = name; // Atribui o nome do logger.
            loggerConfig = config; // Atribui as configurações fornecidas.
        }

        // Método BeginScope cria um escopo de log (não implementado aqui, retorna null).
        public IDisposable? BeginScope<TState>(TState state)
        {
            return null; // Não está usando escopo de log, retorna null.
        }

        // Método IsEnabled verifica se o nível de log está habilitado, de acordo com a configuração.
        public bool IsEnabled(LogLevel logLevel)
        {
            // Verifica se o nível de log atual corresponde ao configurado.

            return logLevel == loggerConfig.LogLevel; 

        }

        // Método Log é responsável por formatar a mensagem e chamar o método que escreve no arquivo.
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception?, string> formatter)
        {
            // Formata a mensagem do log com nível, ID do evento e o estado fornecido.
            string message = $"{logLevel.ToString()} {eventId.Id} - {formatter(state, exception)}";

            // Chama o método para escrever a mensagem no arquivo.
            EscreverTextoNoArquivo(message);
        }

        // Método privado que escreve a mensagem no arquivo de log.
        private void EscreverTextoNoArquivo(string message)
        {
            // Define o caminho do arquivo de log.
            string caminhoArquivoLog = @"C:\Users\Eduardo\Desktop\dados.txt";

            // Cria e abre o arquivo de log para escrita (sempre adicionando ao final).
            using (StreamWriter streamWriter = new StreamWriter(caminhoArquivoLog, true))
            {
                try
                {
                    // Escreve a mensagem no arquivo.
                    streamWriter.WriteLine(message);
                    streamWriter.Close(); // Fecha o arquivo após escrever.
                }
                catch (Exception ex)
                {
                    // Em caso de erro ao escrever no arquivo, lança a exceção.
                    throw;
                }
            }
        }
    }
}
