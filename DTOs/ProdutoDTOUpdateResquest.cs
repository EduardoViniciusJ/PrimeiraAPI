using System.ComponentModel.DataAnnotations;

namespace PrimeiraAPI.DTOs
{
    public class ProdutoDTOUpdateResquest : IValidatableObject
    {
        [Range(1, 9999, ErrorMessage = "Estoque deve estar entre 1 e 9999")]    
        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(DataCadastro.Date <= DateTime.Now.Date)
            {
                yield return new ValidationResult("A data de cadastro não pode ser menor ou igual a data atual",
                    new[]
                    {
                        nameof(this.DataCadastro),
                    });
            }
        }
    }
}
