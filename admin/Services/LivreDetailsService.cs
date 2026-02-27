using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

public class LivreDetailsService : HandlerCRUD<LivreDetails>
{
    private readonly ApplicationDbContext _context;

    public LivreDetailsService(ApplicationDbContext context)
        : base(context, "LivreDetails")
    {
        _context = context;
    }

    public async Task<byte[]> ExportCsvAsync()
    {
        var data = await _context.Set<LivreDetails>()
            .AsNoTracking()
            .ToListAsync();

        var sb = new StringBuilder();
        // 🔹 En-têtes CSV
        sb.AppendLine("LivreId;LivreNom;LivrePhoto;DateEntree;DateEdition;GenreNom;AuteurNom");
        foreach (var item in data)
        {
            sb.AppendLine(
                $"{item.LivreId};" +
                $"{Escape(item.LivreNom)};" +
                $"{Escape(item.LivrePhoto)};" +
                $"{item.DateEntree:yyyy-MM-dd};" +
                $"{item.DateEdition:yyyy-MM-dd};" +
                $"{Escape(item.GenreNom)};" +
                $"{Escape(item.AuteurNom)}"
            );
        }
        // UTF8 avec BOM → Excel friendly
        return Encoding.UTF8.GetPreamble()
            .Concat(Encoding.UTF8.GetBytes(sb.ToString()))
            .ToArray();
    }

    private string Escape(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return "";
        // protège les ; et les "
        return $"\"{value.Replace("\"", "\"\"")}\"";
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
public async Task<List<Livre>> ImportCsvAsync(Stream csvStream)
{
    List<Livre> livresRetournValue = new List<Livre>();
    if (csvStream == null || csvStream.Length == 0)
        throw new ArgumentException("Le flux CSV est vide.", nameof(csvStream));

    try
    {
        // using var reader = new StreamReader(csvStream);
        using var reader = new StreamReader(csvStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
        // string contenu = await reader.ReadToEndAsync();
        // Console.WriteLine(contenu);
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
            Console.WriteLine("Length"+columns.Length);
            if (columns.Length  > 7)
                continue;

            Console.WriteLine(CleanCsvValue(columns[0].Trim()));
            string livreNom = CleanCsvValue(columns[0].Trim());
            string? livrePhoto = CleanCsvValue(columns[1].Trim());
            DateTime? dateEntree = FormatDate(columns[2].Trim());
            DateTime? dateEdition = string.IsNullOrWhiteSpace(columns[3])
                ? null
                :FormatDate(columns[3].Trim());
            string genreNom = CleanCsvValue(columns[4].Trim());
            string auteurNom = CleanCsvValue(columns[5].Trim());

            // ======================
            // GENRE
            // ======================
            var genre = await _context.Genres
                .FirstOrDefaultAsync(g => g.Nom == genreNom);

            if (genre == null)
            {
                genre = new Genre { Nom = genreNom };
                _context.Genres.Add(genre);
                await _context.SaveChangesAsync();
            }

            // ======================
            // AUTEUR
            // ======================
            var auteur = await _context.Auteurs
                .FirstOrDefaultAsync(a => a.Nom == auteurNom);

            if (auteur == null)
            {
                auteur = new Auteur { Nom = auteurNom };
                _context.Auteurs.Add(auteur);
                await _context.SaveChangesAsync();
            }

            // ======================
            // LIVRE
            // ======================
            bool livreExiste = await _context.Livres.AnyAsync(l =>
                l.Nom == livreNom &&
                l.Idgenre == genre.Id &&
                l.Idauteur == auteur.Id);

            if (!livreExiste)
            {
                var livre = new Livre
                {
                    Nom = livreNom,
                    Photo = livrePhoto,
                    Idgenre = genre.Id,
                    Idauteur = auteur.Id,
                    Auteur=auteur.Nom,
                    Genre=genre.Nom,
                    Dateentrebibliotheque = dateEntree,
                    Dateedition = dateEdition
                };
                _context.Livres.Add(livre);
                await _context.SaveChangesAsync();
                livresRetournValue.Add(livre);
                // Ici tu peux appeler la fonction pour enregistrer dans Elasticsearch
                // await _serviceElasticService.SaveLivreAsync(livre);
            }
        }
        return livresRetournValue;
    }
    catch (Exception ex)
    {
        // Log détaillé de l'erreur
        Console.WriteLine($"Erreur lors de l'import CSV : {ex.Message}");
        Console.WriteLine(ex.StackTrace);
        throw; // si tu veux remonter l'exception
    }
}

}
