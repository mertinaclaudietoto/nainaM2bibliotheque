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


        public async Task ImportCsvAsync(Stream csvStream)
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

                string livreNom =  CleanCsvValue(columns[1].Trim());
                string? livrePhoto =  CleanCsvValue(columns[2].Trim());
                DateTime dateEntree = DateTime.Parse(columns[3], CultureInfo.InvariantCulture);
                DateTime? dateEdition = string.IsNullOrWhiteSpace(columns[4])
                    ? null
                    : DateTime.Parse(columns[4], CultureInfo.InvariantCulture);

                string genreNom =  CleanCsvValue(columns[5].Trim());
                string auteurNom =  CleanCsvValue(columns[6].Trim());

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
                        Dateentrebibliotheque = dateEntree,
                        Dateedition = dateEdition
                    };

                    _context.Livres.Add(livre);
                    await _context.SaveChangesAsync();
                }
            }
        }

}
