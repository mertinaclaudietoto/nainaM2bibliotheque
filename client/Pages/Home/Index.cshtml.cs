using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Client.Repositorys;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Nest;

public class LivreModel : PageModel
{
    private readonly string elasticUrl = "http://localhost:9200"; 

    private readonly LivreRepository _livreRepo;
    private const int PageSize = 10; // Taille de page
    private readonly IConfiguration _configuration; 

    public LivreModel(LivreRepository livreRepo,IConfiguration configuration)
    {
        _livreRepo = livreRepo;
        _configuration = configuration;
    }

    /// <summary>
    /// elastique cherche
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public void OnGet(string query = null, int pageNumber = 1, int pageSize = 10)
    {
        var settings = new ConnectionSettings(new Uri(elasticUrl))
            .DefaultIndex("livres");
        var client = new ElasticClient(settings);
        // Calcul de l'offset pour la pagination
        int from = (pageNumber - 1) * pageSize;
        // Construction de la requête Elasticsearch
        var searchResponse = client.Search<LivreDetails>(s => s
            .Query(q =>
                string.IsNullOrWhiteSpace(query)
                    ? q.MatchAll() // si pas de query, retourne tout
                    : q.MultiMatch(m => m
                        .Fields(f => f.Field(ff => ff.LivreNom).Field(ff => ff.AuteurNom))
                        .Query(query)
                        .Fuzziness(Fuzziness.Auto)
                    )
            )
            .From(from)
            .Size(pageSize)
        );
        // Vérifier que la recherche est valide
        List<LivreDetails> resultats = new List<LivreDetails>();
        if (searchResponse.IsValid)
            resultats.AddRange(searchResponse.Documents);
            else
            {
                Console.WriteLine("Erreur Elasticsearch : " + searchResponse.OriginalException?.Message);
                Console.WriteLine("Debug info : " + searchResponse.DebugInformation);
            }
        Livres = resultats;
        // Pagination
        PageNumber = pageNumber;
        Query = query;
        TotalPages = (int)Math.Ceiling((double)searchResponse.Total / pageSize);
    }



    // Liste des livres à afficher
        public List<LivreDetails> Livres { get; set; } = new List<LivreDetails>();

        // Pagination
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int TotalPages { get; set; } = 1;
        public string Query { get; set; } = "";


        // Optional: filtre par genre
        [BindProperty(SupportsGet = true)]
        public int? GenreId { get; set; }

   

    public async Task<IActionResult> OnPostSelectionnerLivreAsync(int livreId)
    {
        Console.WriteLine("validation");
        // Ici, tu peux appeler ta fonction avec l'ID du livre
        await empruntLivre(livreId);
        // Rediriger ou retourner la même page
        return RedirectToPage();
    }
    private async Task empruntLivre(int livreId)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            throw new Exception("Utilisateur non connecté.");
        }
        string connectionString = _configuration.GetConnectionString("_connectionString");
        using (var conn = new SqlConnection(connectionString))
        {
            await conn.OpenAsync();
            // Vérifier si un emprunt existe déjà pour ce livre sans retour
            var checkCmd = new SqlCommand(@"
                SELECT COUNT(*) 
                FROM emprunt 
                WHERE  idlivre = @idlivre 
                AND dateretouredelivre IS NULL
            ", conn);
            checkCmd.Parameters.AddWithValue("@idlivre", livreId);
            int count = (int)await checkCmd.ExecuteScalarAsync();
            if (count > 0)
            {
                Console.WriteLine("Cet livre a ete déjà  emprunté  et n'est pas encore rendu.");
                TempData["MessagePopup"] = "Cet livre a été déjà emprunté et n'est pas encore rendu.";
                return; // ou lever une exception si tu veux
            }
            // Si pas d'emprunt en cours, insérer le nouvel emprunt
            DateTime dateEmprunt = DateTime.Now;
            DateTime dateLimite = dateEmprunt.AddDays(14);
            var insertCmd = new SqlCommand(@"
                INSERT INTO emprunt (iduser, idlivre, dateemprunt, datelimite, dateretouredelivre)
                VALUES (@iduser, @idlivre, @dateemprunt, @datelimite, NULL)
            ", conn);
            insertCmd.Parameters.AddWithValue("@iduser", userId.Value);
            insertCmd.Parameters.AddWithValue("@idlivre", livreId);
            insertCmd.Parameters.AddWithValue("@dateemprunt", dateEmprunt);
            insertCmd.Parameters.AddWithValue("@datelimite", dateLimite);
            await insertCmd.ExecuteNonQueryAsync();
            TempData["MessagePopup"] = "Emprunt effectué avec succès !";
            Console.WriteLine($"Emprunt créé pour l'utilisateur {userId} et le livre {livreId}");
        }
    }


}
