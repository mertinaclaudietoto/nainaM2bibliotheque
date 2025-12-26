public class LivreEmpruntVM
{
    public int LivreId { get; set; }
    public string LivreNom { get; set; }
    public string LivrePhoto { get; set; }

    public string AuteurNom { get; set; }
    public string GenreNom { get; set; }

    public DateTime? DateEmprunt { get; set; }
    public DateTime? DateLimite { get; set; }
    public DateTime? DateRetour { get; set; }
}
