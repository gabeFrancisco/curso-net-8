using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ApiCatalogo.Validation;

public class Produto : IValidatableObject
{
    [Key]
    public int ProdutoId { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório!")]
    [StringLength(80)]
    [PrimeiraLetraMaiuscula]
    public string? Nome { get; set; }

    [Required]
    [StringLength(300)]
    public string? Descricao { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Preco { get; set; }

    [Required]
    [StringLength(300)]
    public string? ImageUrl { get; set; }
    public float Estoque { get; set; }
    public DateTime DataCadastro { get; set; }

    public int CategoriaId { get; set; }

    [JsonIgnore]
    public Categoria? Categoria { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrEmpty(this.Nome))
        {
            var primeiraLetra = this.Nome[0].ToString();
            if(primeiraLetra != primeiraLetra.ToUpper())
            {
                yield return new ValidationResult("A primeira letra do produto deve ser maiúscula!",
                new[]
                {
                    nameof(this.Nome)
                });
            }
        }
    }
}
