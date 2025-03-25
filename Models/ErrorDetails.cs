using System.Text.Json;

namespace PrimeiraAPI.Models
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public string? Trace { get; set; } // Pilha de erros
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
        

    }
}
