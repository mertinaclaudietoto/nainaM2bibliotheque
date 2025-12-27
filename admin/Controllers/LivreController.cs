using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using adminBibliotheque.Models;
using Azure;

namespace adminBibliotheque.Controllers;

public class LivreController : Controller
{
    private readonly ILogger<LivreController> _logger;
    private readonly LivreService _service;
    private readonly GenreService _serviceGenre;
    private readonly AuteurService _serviceAuteur;
    private readonly LivreDetailsService _serviceLivre;



    public LivreController(ILogger<LivreController> logger,LivreService service,GenreService serviceGenre,AuteurService serviceAuteur,LivreDetailsService serviceLivre)
    {
        _logger = logger;
        _service=service;
        _serviceGenre=serviceGenre;
        _serviceAuteur=serviceAuteur;
        _serviceLivre=serviceLivre;

    }

    // public IActionResult Index()
    // {
    //     return View();
    // }
     public IActionResult Move(int move = 999)
    {
        return Content("Valeur reçue = " + move);
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
        var livres = await _service.selectLivres(pageNumber, pageSize);
        var data = new LivreIndexViewModel
        {
            Livres = livres,
            Genres= await  _serviceGenre.getAll(),
            Auteurs =  await _serviceAuteur.getAll(),
        };

        return View(data);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    // POST : Ajout ou Update
    [HttpPost]
    public async Task<IActionResult> Save(LivreIndexViewModel model)
    {
        // Console.WriteLine(ModelState);
        // if (!ModelState.IsValid)
        // {
        //     // Recharger les listes pour le formulaire
        //     model.Auteurs = await _serviceAuteur.getAll();
        //     model.Genres = await _serviceGenre.getAll();
        //     var livres = await _service.selectLivres(1, 10);
        //     model.Livres = livres;
        //     return View("Index", model);
        // }
        // Mapping vers l'entité Livre
        var livre = new Livre
        {
            Id = model.Id,
            Nom = model.Nom,
            Photo = model.Photo,
            Idauteur = model.Idauteur,
            Idgenre = model.Idgenre,
            Dateedition = model.Dateedition,
            Dateentrebibliotheque = DateTime.Now
        };

        if (livre.Id == 0)
        {
             livre.Id=null;
            await _service.SaveAsync(livre);   // Insert
        }
        else
            await _service.UpdateAsync(livre); // Update

        return RedirectToAction("Index");
    }

      public async Task<IActionResult> DeleteV(int id)
    {

        await _service.Delete(id);

        return RedirectToAction("Index");
    }
    [HttpGet]
    public async Task<IActionResult> ExportCsv()
    {
        var csvBytes = await _serviceLivre.ExportCsvAsync();

        return File(
            csvBytes,
            "text/csv",
            $"livres_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
        );
    }
    [HttpPost]
    public async Task<IActionResult> ImportCsv(IFormFile csvFile)
    {
        if (csvFile == null || csvFile.Length == 0)
        {
            TempData["Error"] = "Veuillez sélectionner un fichier CSV.";
            return RedirectToAction("Index");
        }
        if (!csvFile.FileName.EndsWith(".csv"))
        {
            TempData["Error"] = "Le fichier doit être au format CSV.";
            return RedirectToAction("Index");
        }
        using var stream = csvFile.OpenReadStream();
        await _serviceLivre.ImportCsvAsync(stream);

        TempData["Success"] = "Import CSV effectué avec succès.";
        return RedirectToAction("Index");
    }
    
}


