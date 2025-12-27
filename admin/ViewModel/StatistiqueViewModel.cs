using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

    
public class StatistiqueViewModel
{
    public  List<StatAge> statAge { get; set; }
   public   List<StatAuteur> statAuteur { get; set; }
    public     List<StatEmprunt> statEmprunt { get; set; }
     public   List<StatGenre> statGenre { get; set; }
}
