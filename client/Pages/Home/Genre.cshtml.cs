using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Client.Repositorys;

public class GenreModel : PageModel
{
    private readonly GenreRepository _genreRepo;

    public GenreModel(GenreRepository genreRepo)
    {
        _genreRepo = genreRepo;
    }

    public List<Genre> Genres { get; set; } = new List<Genre>();

    public void OnGet()
    {
        Genres = _genreRepo.GetAll();
    }
}
