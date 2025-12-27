public class LivreDetails
{
    public int LivreId { get; set; }
    public string LivreNom { get; set; } = string.Empty;
    public string LivrePhoto { get; set; } = string.Empty;
    public DateTime DateEntree { get; set; }
    public DateTime? DateEdition { get; set; }

    public int GenreId { get; set; }
    public string GenreNom { get; set; } = string.Empty;

    public int AuteurId { get; set; }
    public string AuteurNom { get; set; } = string.Empty;
}
