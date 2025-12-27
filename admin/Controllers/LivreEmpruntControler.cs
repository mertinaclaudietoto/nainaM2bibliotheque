using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using adminBibliotheque.Models;

namespace adminBibliotheque.Controllers;

public class LivreEmpruntController : Controller
{
    private readonly ILogger<LivreEmpruntController> _logger;
    private readonly LivreEmpruntService _service;
    private readonly UsersService _serviceUsers;



    public LivreEmpruntController(ILogger<LivreEmpruntController> logger,LivreEmpruntService service,UsersService serviceUsers)
    {
        _logger = logger;
        _service=service;
     _serviceUsers= serviceUsers;
    }

     public async Task<IActionResult> Index(int nextorprevious = 0 )
    {
        Console.WriteLine(nextorprevious);
        int pageSize = 10;
        int pageNumber = HttpContext.Session.GetInt32("pageNumberLivre") ?? 1;
        pageNumber += nextorprevious;

        Console.WriteLine(pageNumber);

        if (pageNumber < 1)
            pageNumber = 1;
        HttpContext.Session.SetInt32("pageNumberLivre", pageNumber);
        var livres = await _service.GetPagedAsync(pageNumber, pageSize);
        var data = new LivreEmpruntIndexViewModel
        {
            LivresEmprunt = livres,
        };

        return View(data);
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public IActionResult Statistique()
    {
          var data = new StatistiqueViewModel

        {
            statAge =   _serviceUsers.GetPourcentageMembreParAge(),
            statAuteur =_service.GetPourcentageEmpruntParAuteur(),
            statEmprunt = _service.GetNombreEmpruntParMois(new DateTime().Year),
            statGenre =_service.GetPourcentageEmpruntParGenre()
        };
        return View(data);
    }
}
