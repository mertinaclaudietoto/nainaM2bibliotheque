using Nest;
public class LivreDetails
{
    [PropertyName("id")]
    public int LivreId { get; set; }
    [PropertyName("nom")]
    public string LivreNom { get; set; } = string.Empty;
    [PropertyName("photo")]
    public string LivrePhoto { get; set; } = string.Empty;
    [PropertyName("dateentrebibliotheque")]
    public DateTime DateEntree { get; set; }
    [PropertyName("dateedition")]
    public DateTime? DateEdition { get; set; }
    [PropertyName("idgenre")]
    public int GenreId { get; set; }
    [PropertyName("genre")]

    public string GenreNom { get; set; } = string.Empty;

    public int AuteurId { get; set; }
    [PropertyName("auteur")]
    public string AuteurNom { get; set; } = string.Empty;
}
