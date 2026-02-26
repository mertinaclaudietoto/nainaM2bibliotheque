using Nest;
using System;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

public class ElasticService
{
    private readonly ElasticClient _client;
    private readonly string elasticUrl = "http://localhost:9200"; 
    private readonly ApplicationDbContext _context;

    public ElasticService(ApplicationDbContext context )
    {
        _context=context;
        var settings = new ConnectionSettings(new Uri(elasticUrl))
            .DefaultIndex("livres");
        _client = new ElasticClient(settings);
    }

    public ElasticClient GetClient()
    {
        return _client;
    }
    public void SaveLivre()
    { }
    public List<Livre> SearchLivre(string query, int page = 1, int pageSize = 10)
    {
        var settings = new ConnectionSettings(new Uri(elasticUrl))
            .DefaultIndex("livres");
        var client = new ElasticClient(settings);
        // Calcul de l'offset
        int from = (page - 1) * pageSize;
        // Recherche avec pagination
        var searchResponse = client.Search<Livre>(s => s
            .Query(q => q
                .MultiMatch(m => m
                    .Fields(f => f.Field(ff => ff.Nom).Field(ff => ff.Auteur))
                    .Query(query)
                )
            )
            .From(from)       // sauter les résultats des pages précédentes
            .Size(pageSize)   // nombre de résultats par page
        );

        List<Livre> resultats = new List<Livre>();
        if (searchResponse.IsValid)
            resultats.AddRange(searchResponse.Documents);

        return resultats;
    }

    public void SaveLivreIndex(LivreIndexViewModel livre)
    {
        try
        {
            var auteurData = livre.Idauteur.ToString().Split('|');
            var genreData = livre.Idgenre.ToString().Split('|');
            // Création du client Elasticsearch
            var settings = new Nest.ConnectionSettings(new Uri(elasticUrl))
                .DefaultIndex("livres"); // index par défaut
            var client = new Nest.ElasticClient(settings);
            // Indexer le document dans Elasticsearch
            var indexResponse = client.IndexDocument(new
            {
                Id = livre.Id,
                Nom = livre.Nom,
                Photo = livre.Photo,
                Idauteur = livre.Idauteur,
                Auteur= auteurData[1],
                Genre = genreData[1],
                Idgenre = livre.Idgenre,
                Dateedition = livre.Dateedition,
                Dateentrebibliotheque = DateTime.Now
            });
            if (!indexResponse.IsValid)
            {
                // Optionnel : log l'erreur
                Console.WriteLine("Erreur Elasticsearch : " + indexResponse.ServerError);
            }
        }
        catch (Exception ex)
        {
            // Optionnel : log l'exception
            Console.WriteLine("Exception Elasticsearch : " + ex.Message);
        }
    }
     private string CleanCsvValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "";

        return value.Trim().Trim('"');
    }
    public async Task SaveImportCSV(Stream csvStream)
    {
        using var reader = new StreamReader(csvStream);
        string? line;
        bool isFirstLine = true;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            // Ignorer l'en-tête
            if (isFirstLine)
            {
                isFirstLine = false;
                continue;
            }
            var columns = line.Split(';');
            if (columns.Length < 7)
                continue;
            //ajout de l'index 
            var settings = new Nest.ConnectionSettings(new Uri(elasticUrl))
                .DefaultIndex("livres"); 
            var client = new Nest.ElasticClient(settings);
            var indexResponse = client.IndexDocument(new
            {
                Nom = CleanCsvValue(columns[1].Trim()),
                Photo = CleanCsvValue(columns[2].Trim()),
                Auteur= CleanCsvValue(columns[6].Trim()),
                Genre = CleanCsvValue(columns[5].Trim()),
                Dateedition = DateTime.Parse(columns[3], CultureInfo.InvariantCulture),
                Dateentrebibliotheque = DateTime.Now
            });
            if (!indexResponse.IsValid)
            {
                // Optionnel : log l'erreur
                Console.WriteLine("Erreur Elasticsearch : " + indexResponse.ServerError);
            }
        }
    }

}
