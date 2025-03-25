using Microsoft.AspNetCore.Mvc.Filters;


namespace PrimeiraAPI.Filters
{
    public class ApiLogginFilter : IActionFilter
    {
        private readonly ILogger<ApiLogginFilter> _logger;

        public ApiLogginFilter(ILogger<ApiLogginFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // executa antes do Action
            _logger.LogInformation("#### Executando -> OnActionExecuting");
            _logger.LogInformation("######");
            _logger.LogInformation($"{DateTime.Now.ToShortTimeString()}");
            _logger.LogInformation($"ModelState: {context.ModelState.IsValid}");
            _logger.LogInformation("######");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // executa  depois do Action
            _logger.LogInformation("#### Executando -> OnActionExecuted");
            _logger.LogInformation("######");
            _logger.LogInformation($"{DateTime.Now.ToShortTimeString()}");
            _logger.LogInformation($"ModelState: {context.HttpContext.Response.StatusCode}");
            _logger.LogInformation("######");

        }
    }
}
