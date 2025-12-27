using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("v_livre_emprunt")]
public class LivreEmprunt
{
    [Key]
    [Column("LivreId")]
    public int LivreId { get; set; }

    [Column("LivreNom")]
    public string LivreNom { get; set; }

    [Column("LivrePhoto")]
    public string? LivrePhoto { get; set; }

    [Column("DateEntree")]
    public DateTime DateEntree { get; set; }

    [Column("DateEdition")]
    public DateTime? DateEdition { get; set; }

    [Column("GenreId")]
    public int GenreId { get; set; }

    [Column("GenreNom")]
    public string GenreNom { get; set; }

    [Column("AuteurId")]
    public int AuteurId { get; set; }

    [Column("AuteurNom")]
    public string AuteurNom { get; set; }

    // Colonnes liées à l'emprunt
    [Column("dateemprunt")]
    public DateTime? DateEmprunt { get; set; }

    [Column("datelimite")]
    public DateTime? DateLimite { get; set; }

    [Column("dateretouredelivre")]
    public DateTime? DateRetourDeLivre { get; set; }

    // Colonnes liées à l'utilisateur
    [Column("nom")]
    public string? NomUtilisateur { get; set; }

    [Column("prenom")]
    public string? PrenomUtilisateur { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("login")]
    public string? Login { get; set; }
}
