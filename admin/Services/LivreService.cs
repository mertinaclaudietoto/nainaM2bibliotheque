
using Microsoft.EntityFrameworkCore;

    public class LivreService :HandlerCRUD<Livre>
    {
        private readonly  ApplicationDbContext _context;

        public LivreService( ApplicationDbContext context): base(context, "Livres")
        {
            _context = context;
        }
          public async Task<bool> Delete(int id)
    {
        var livre = await _context.Livres.FindAsync(id);
        if (livre == null)
            return false; // pas trouvé

        _context.Livres.Remove(livre);
        await _context.SaveChangesAsync();
        return true;
    }
       public async Task<List<Livre>> selectLivres( int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            return await (
                from pc in _context.Livres
                join c in _context.Auteurs
                    on pc.Idauteur equals c.Id
                join g in _context.Genres
                    on pc.Idgenre equals g.Id
                select new Livre
                {
                    Id = pc.Id,
                    Photo =pc.Photo,
                    Nom=pc.Nom,
                    Idgenre=pc.Idgenre,
                    Idauteur=pc.Idauteur,
                    Auteur=c.Nom,
                    Genre=g.Nom
                }
            )
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        }
        
         



    }