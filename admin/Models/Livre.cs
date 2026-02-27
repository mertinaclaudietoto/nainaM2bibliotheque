using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Nest;

    
    [Table("livre")]
    public class Livre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        [PropertyName("_id")]
        public int? Id { get; set; }

        [Column("photo")]
        [PropertyName("photo")]
        public string? Photo { get; set; }

        [Column("nom")]
        [PropertyName("nom")]
        public string Nom { get; set; }

        [Column("idgenre")]
        [PropertyName("idgenre")]
        public int Idgenre { get; set; }

        [Column("idauteur")]
        [PropertyName("idauteur")]
        public int Idauteur { get; set; }

        [NotMapped]
        public string Auteur;
        [NotMapped]
        public string Genre;
        [Column("dateentrebibliotheque")]
        [PropertyName("dateentrebibliotheque")]
        public DateTime? Dateentrebibliotheque { get; set; }

        [Column("dateedition")]
        [PropertyName("dateedition")]
        public DateTime? Dateedition { get; set; }

      
    }

