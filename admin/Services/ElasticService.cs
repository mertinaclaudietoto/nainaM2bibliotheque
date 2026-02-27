using Nest;
using Elasticsearch.Net;
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

    public  async void SaveLivreIndex(Livre livre)
    {
        try
        {
            // Création du client Elasticsearch
            var settings = new Nest.ConnectionSettings(new Uri(elasticUrl))
                .DefaultIndex("livres"); // index par défaut
            var client = new Nest.ElasticClient(settings);
            var indexResponse = await client.IndexAsync(new 
                {
                    Nom = livre.Nom,
                    Photo = livre.Photo,
                    Idauteur = livre.Idauteur,
                    Auteur = livre.Auteur,
                    Genre = livre.Genre,
                    Idgenre = livre.Idgenre,
                    Dateedition = livre.Dateedition,
                    Dateentrebibliotheque = DateTime.Now
                }, i => i.Id(livre.Id)); // <--  fixes le _id
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
    public async void UpdateLivreIndex(Livre livre)
    {
        try
        {
            // Création du client Elasticsearch
            var settings = new Nest.ConnectionSettings(new Uri(elasticUrl))
                .DefaultIndex("livres"); // index par défaut
            var client = new Nest.ElasticClient(settings);
            // Update du document existant avec le _id
            var updateResponse = await client.UpdateAsync<object>(livre.Id, u => u
                .Index("livres")
                .Doc(new 
                {
                    Nom = livre.Nom,
                    Photo = livre.Photo,
                    Idauteur = livre.Idauteur,
                    Auteur = livre.Auteur,
                    Genre = livre.Genre,
                    Idgenre = livre.Idgenre,
                    Dateedition = livre.Dateedition,
                    Dateentrebibliotheque = DateTime.Now
                })
                .DocAsUpsert(true) // si le document n'existe pas, le créer
            );
            if (!updateResponse.IsValid)
            {
                // Optionnel : log l'erreur
                Console.WriteLine("Erreur Elasticsearch : " + updateResponse.ServerError);
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
     public DateTime? FormatDate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        string[] formats = { "dd/MM/yyyy", "yyyy-MM-dd" ,"dd-MM-yyyy","yyyy/MM/dd"};

        if (DateTime.TryParseExact(value.Trim(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            return date;
        }
        else
        {
            Console.WriteLine($"Format date invalide : {value}");
            return null;
        }
    }

    public async Task SaveImportCSV(List<Livre> listelivre)
    {
        var settings = new Nest.ConnectionSettings(new Uri(elasticUrl))
            .DefaultIndex("livres"); 
        var client = new Nest.ElasticClient(settings);
        foreach (var value in listelivre)
        {
             var indexResponse = await client.IndexAsync(new 
                {
                    Nom = value.Nom,
                    Photo = value.Photo,
                    Idauteur = value.Idauteur,
                    Auteur = value.Auteur,
                    Genre = value.Genre,
                    Idgenre = value.Idgenre,
                    Dateedition = value.Dateedition,
                    Dateentrebibliotheque = DateTime.Now
                }, i => i.Id(value.Id)); // <--  fixes le _id
            if (!indexResponse.IsValid)
            {
                // Optionnel : log l'erreur
                Console.WriteLine("Erreur Elasticsearch : " + indexResponse.ServerError);
            }
        }
    }


    public async Task<bool> DeleteDocumentAsync(string indexName, int id)
    {
        var settings = new ConnectionSettings(new Uri(elasticUrl))
                        .DefaultIndex(indexName);
        var client = new ElasticClient(settings);
        var response = await client.DeleteAsync<Livre>(id, d => d.Index(indexName));
        return response.IsValid;
    }

}
