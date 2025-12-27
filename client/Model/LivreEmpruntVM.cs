using System;
public class LivreEmpruntVM
{
    public int LivreId { get; set; }
    public string LivreNom { get; set; }
    public string? LivrePhoto { get; set; }

    public DateTime DateEntree { get; set; }
    public DateTime? DateEdition { get; set; }

    public int GenreId { get; set; }
    public string GenreNom { get; set; }

    public int AuteurId { get; set; }
    public string AuteurNom { get; set; }

    public int IdUser { get; set; }

    // Colonnes liées à l'emprunt
    public DateTime? DateEmprunt { get; set; }
    public DateTime? DateLimite { get; set; }
    public DateTime? DateRetourDeLivre { get; set; }

    // Colonnes liées à l'utilisateur
    public string? NomUtilisateur { get; set; }
    public string? PrenomUtilisateur { get; set; }
    public string? Email { get; set; }
    public string? Login { get; set; }
}
