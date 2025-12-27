
using Microsoft.EntityFrameworkCore;

    public class LivreEmpruntService :HandlerCRUD<LivreEmprunt>
    {
        private readonly  ApplicationDbContext _context;

        public LivreEmpruntService( ApplicationDbContext context): base(context, "LivreEmprunt")
        {
            _context = context;
        }
        public List<StatEmprunt> GetNombreEmpruntParMois(int annee)
        {
            var result = _context.LivreEmprunt
                .Where(e => e.DateEmprunt.HasValue && e.DateEmprunt.Value.Year == annee)
                .GroupBy(e => new
                {
                    Year = e.DateEmprunt.Value.Year,
                    Month = e.DateEmprunt.Value.Month
                })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    Nbr = g.Count()
                })
                .AsEnumerable() // 🔥 bascule côté C#
                .Select(x => new StatEmprunt
                {
                    date = new DateTime(x.Year, x.Month, 1),
                    nbr = x.Nbr
                })
                .OrderBy(s => s.date)
                .ToList();

            return result;
        }

        public List<StatAuteur> GetPourcentageEmpruntParAuteur()
        {
            var totalEmprunts = _context.LivreEmprunt
                .Count(e => e.DateEmprunt.HasValue);

            if (totalEmprunts == 0)
                return new List<StatAuteur>();

            var result = _context.LivreEmprunt
                .Where(e => e.DateEmprunt.HasValue)
                .Join(_context.Livres,
                    e => e.LivreId,
                    l => l.Id,
                    (e, l) => l.Idauteur)
                .Join(_context.Auteurs,
                    aid => aid,
                    a => a.Id,
                    (aid, a) => a.Nom)
                .GroupBy(auteurNom => auteurNom)
                .Select(g => new StatAuteur
                {
                    auteur = g.Key,
                    pourcentage = Math.Round(
                        (double)g.Count() * 100 / totalEmprunts, 2
                    )
                })
                .Where(s => s.pourcentage > 0)
                .OrderByDescending(s => s.pourcentage)
                .ToList();

            return result;
        }

        public List<StatGenre> GetPourcentageEmpruntParGenre()
        {
            // Total des emprunts réellement effectués
            var totalEmprunts = _context.LivreEmprunt
                .Count(e => e.DateEmprunt.HasValue);

            if (totalEmprunts == 0)
                return new List<StatGenre>();

            var result = _context.LivreEmprunt
                .Where(e => e.DateEmprunt.HasValue)
                .Join(_context.Livres,
                    e => e.LivreId,
                    l => l.Id,
                    (e, l) => l.Idgenre)
                .Join(_context.Genres,
                    gid => gid,
                    g => g.Id,
                    (gid, g) => g.Nom)
                .GroupBy(genreNom => genreNom)
                .Select(g => new StatGenre
                {
                    genre = g.Key,
                    pourcentage = Math.Round(
                        (double)g.Count() * 100 / totalEmprunts, 2
                    )
                })
                .Where(s => s.pourcentage > 0)
                .OrderByDescending(s => s.pourcentage)
                .ToList();

            return result;
        }

        public List<LivreEmprunt> GetEmpruntsParUtilisateur(int idUser)
            {
                var emprunts = _context.LivreEmprunt
                    .Where(e => e.DateEmprunt.HasValue && e.IdUser == idUser)
                    .OrderByDescending(e => e.DateEmprunt)
                    .ToList();

                return emprunts;
            }

}