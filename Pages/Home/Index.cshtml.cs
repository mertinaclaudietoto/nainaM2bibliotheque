using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Client.Repositorys;
public class LivreModel : PageModel
{
    private readonly LivreRepository _livreRepo;
    private const int PageSize = 10; // Taille de page

    public LivreModel(LivreRepository livreRepo)
    {
        _livreRepo = livreRepo;
    }

    // Liste des livres à afficher
    public List<LivreDetails> Livres { get; set; } = new List<LivreDetails>();

    // Pagination
    [BindProperty(SupportsGet = true)]
    public int PageNumber { get; set; } = 1;

    public int TotalPages { get; set; } = 1;

    // Optional: filtre par genre
    [BindProperty(SupportsGet = true)]
    public int? GenreId { get; set; }

    public void OnGet()
    {
        // Récupère le total de livres pour calculer le nombre de pages
        int totalLivres;
        if (GenreId.HasValue)
        {
            totalLivres = _livreRepo.GetByGenre(GenreId.Value).Count;
            Livres = _livreRepo.GetPaged(PageNumber, PageSize, GenreId);
        }
        else
        {
            totalLivres = _livreRepo.GetAll().Count;
            Livres = _livreRepo.GetPaged(PageNumber, PageSize);
        }

        TotalPages = (int)Math.Ceiling((double)totalLivres / PageSize);
    }
}
