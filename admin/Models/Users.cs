using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("users")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("nom")]
    [MaxLength(255)]
    public string Nom { get; set; }

    [Required]
    [Column("prenom")]
    [MaxLength(255)]
    public string Prenom { get; set; }

    [Column("email")]
    [MaxLength(255)]
    public string Email { get; set; }

    [Required]
    [Column("login")]
    [MaxLength(255)]
    public string Login { get; set; }

    [Required]
    [Column("motdepasse")]
    [MaxLength(255)]
    public string MotDePasse { get; set; }

    [Column("datedenaissance")]
    public DateTime? DateDeNaissance { get; set; }


    [Column("dateinscription")]
    public DateTime Dateinscription { get; set; }
}
