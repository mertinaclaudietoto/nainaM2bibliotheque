using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

    
public class LivreIndexViewModel
{
    public int Id { get; set; }

    [Required]
    public string Nom { get; set; }

    [Required]
    public string Photo { get; set; }

    [Required]
    public int Idauteur { get; set; }

    [Required]
    public int Idgenre { get; set; }

    [Required]
    public DateTime Dateedition { get; set; }

    public List<Livre> Livres { get; set; }

    public List<Auteur> Auteurs { get; set; }
    public List<Genre> Genres { get; set; }

    // (optionnel) filtres sélectionnés
    public int? AuteurId { get; set; }
    public int? GenreId { get; set; }
}
