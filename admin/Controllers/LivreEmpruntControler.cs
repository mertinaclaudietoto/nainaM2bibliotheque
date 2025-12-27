using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using adminBibliotheque.Models;
using System.Threading.Tasks;

namespace adminBibliotheque.Controllers;

public class LivreEmpruntController : Controller
{
    private readonly ILogger<LivreEmpruntController> _logger;
    private readonly LivreEmpruntService _service;
    private readonly UsersService _serviceUsers;
    private readonly PdfService  _pdfService;




    public LivreEmpruntController(ILogger<LivreEmpruntController> logger,LivreEmpruntService service,UsersService serviceUsers,PdfService  pdfService)
    {
        _logger = logger;
        _service=service;
        _serviceUsers= serviceUsers;
        _pdfService=pdfService;
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


    [HttpGet("api/livreemprunt/utilisateur/{idUser}")]
    public IActionResult GetEmpruntsParUtilisateur(int idUser)
    {
        var emprunts = _service.GetEmpruntsParUtilisateur(idUser);

        if (emprunts == null || !emprunts.Any())
            return NotFound(new { message = "Aucun emprunt trouvé pour cet utilisateur." });

        return Ok(emprunts);
    }
    [HttpGet("admin-bibliophilia/pdf/{idUser}")]
    public async Task<IActionResult> GenererPdfCarte(int idUser)
    {
        var user = await _serviceUsers.GetById(idUser);
        if (user == null) return NotFound();

        DateTime dateInscription = DateTime.Today; // ou prendre la vraie date si disponible
        var pdfBytes = _pdfService.GenererCarteBibliotheque(user, dateInscription);

        return File(pdfBytes, "application/pdf", $"{user.Nom}_{user.Prenom}_Carte.pdf");
    }


}
