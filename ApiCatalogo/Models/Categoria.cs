using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

public class Categoria
{
    public int CategoriaId { get; set; }

    [Required]
    [StringLength(80)]
    public string? Nome { get; set; }

    [Required]
    [StringLength(300)]
    public string? ImageUrl { get; set; }

    public ICollection<Produto>? Produtos { get; set; }

    public Categoria()
    {
        Produtos = new Collection<Produto>();
    }

}
