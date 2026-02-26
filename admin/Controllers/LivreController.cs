using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using adminBibliotheque.Models;
using Azure;
using Nest;
using System.Collections.Generic;
namespace adminBibliotheque.Controllers;

public class LivreController : Controller
{
    private readonly string elasticUrl = "http://localhost:9200"; 
    private readonly ILogger<LivreController> _logger;
    private readonly LivreService _service;
    private readonly GenreService _serviceGenre;
    private readonly AuteurService _serviceAuteur;
    private readonly LivreDetailsService _serviceLivre;
    private readonly ElasticService _serviceElasticService;

    public LivreController(ElasticService serviceElasticService,ILogger<LivreController> logger,LivreService service,GenreService serviceGenre,AuteurService serviceAuteur,LivreDetailsService serviceLivre)
    {
        _logger = logger;
        _service=service;
        _serviceGenre=serviceGenre;
        _serviceAuteur=serviceAuteur;
        _serviceLivre=serviceLivre;
        _serviceElasticService=serviceElasticService;
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
        int pageSize = 10;
        int pageNumber = HttpContext.Session.GetInt32("pageNumberLivre") ?? 1;
        pageNumber += nextorprevious;

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
        Console.WriteLine(model.Idauteur.ToString());
        var auteurData = model.Idauteur.ToString().Split('|');
        var genreData = model.Idgenre.ToString().Split('|');
        Console.WriteLine(auteurData);
        var livre = new Livre
        {
            Id = model.Id,
            Nom = model.Nom,
            Photo = model.Photo,
            Idauteur = int.Parse(auteurData[0]),
            Idgenre =int.Parse(genreData[0]),
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
            _serviceElasticService.SaveLivreIndex(model);
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
        await _serviceElasticService.SaveImportCSV(stream);
        TempData["Success"] = "Import CSV effectué avec succès.";
        return RedirectToAction("Index");
    }
    /// <summary>
    /// elastique cherche
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IActionResult> Rechercher(string query, int nextorprevious = 1, int pageSize = 10)
    {
        var listeLivreByElastiqueSearch= _serviceElasticService.SearchLivre(query,nextorprevious,pageSize);
        int pageNumber = HttpContext.Session.GetInt32("pageNumberLivre") ?? 1;
        pageNumber += nextorprevious;
        if (pageNumber < 1)
            pageNumber = 1;
        HttpContext.Session.SetInt32("pageNumberLivre", pageNumber);
        var data = new LivreIndexViewModel
        {
            Livres = listeLivreByElastiqueSearch,
            Genres= await  _serviceGenre.getAll(),
            Auteurs =  await _serviceAuteur.getAll(),
        };
        return View(data);
    }

}


