using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

public class HistoriqueEmpruntModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    public HistoriqueEmpruntModel(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    public List<LivreEmpruntVM> Emprunts { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
        {
            return RedirectToPage("Users/Login");
        }

        await LoadEmpruntsFromApiAsync(userId.Value);

        return Page();
    }
    // Nouvelle fonction pour récupérer les emprunts depuis l'API REST
    private async Task LoadEmpruntsFromApiAsync(int idUser)
    {
        var client = _httpClientFactory.CreateClient();
        
        string apiUrl = $"http://localhost:5124/api/livreemprunt/utilisateur/{idUser}";
        Console.WriteLine(apiUrl);

        try
        {
            var empruntsFromApi = await client.GetFromJsonAsync<List<LivreEmpruntVM>>(apiUrl);

            if (empruntsFromApi != null)
            {
                Emprunts = empruntsFromApi;
            }
        }
        catch (HttpRequestException ex)
        {
            // Gestion des erreurs (API inaccessible, etc.)
            Console.WriteLine($"Erreur lors de l'appel à l'API : {ex.Message}");
            Emprunts = new List<LivreEmpruntVM>();
        }
    }
     // Exemple : fonction qui récupère le PDF depuis l'API
    public async Task<byte[]> GetPdfCarteAsync()
    {
        int? idUser=HttpContext.Session.GetInt32("UserId");
        var client = _httpClientFactory.CreateClient();
        
        // URL de ton endpoint PDF
        string pdfUrl = $"http://localhost:5124/admin-bibliophilia/pdf/{idUser}";

        try
        {
            var response = await client.GetAsync(pdfUrl);

            response.EnsureSuccessStatusCode(); // lève une exception si erreur HTTP

            // Lire le contenu en bytes
            var pdfBytes = await response.Content.ReadAsByteArrayAsync();
            return pdfBytes;
        }
        catch (HttpRequestException ex)
        {
            // Gestion des erreurs
            Console.WriteLine($"Erreur lors de l'appel au PDF : {ex.Message}");
            return null;
        }
    }
    // Dans HistoriqueEmpruntModel
    public async Task<IActionResult> OnGetTelechargerPdfAsync()
    {
        var pdfBytes = await GetPdfCarteAsync();

        if (pdfBytes == null || pdfBytes.Length == 0)
        {
            return Content("Impossible de récupérer le PDF.");
        }

        // Renvoyer le PDF pour téléchargement
        int? idUser = HttpContext.Session.GetInt32("UserId");
        string fileName = idUser != null ? $"Carte_{idUser}.pdf" : "Carte.pdf";

        return File(pdfBytes, "application/pdf", fileName);
    }

}
