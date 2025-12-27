using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

    
    [Table("livre")]
    public class Livre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("photo")]
    
        public string? Photo { get; set; }

        [Column("nom")]
        public string Nom { get; set; }

        [Column("idgenre")]
        public int Idgenre { get; set; }

        [Column("idauteur")]
        public int Idauteur { get; set; }

        [NotMapped]
        public string Auteur;
        [NotMapped]
        public string Genre;


        [Column("dateentrebibliotheque")]
        public DateTime Dateentrebibliotheque { get; set; }

        [Column("dateedition")]
        public DateTime? Dateedition { get; set; }

      
    }

