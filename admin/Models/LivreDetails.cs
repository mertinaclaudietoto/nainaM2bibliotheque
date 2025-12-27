using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("vw_LivreDetails")]
public class LivreDetails
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
}
