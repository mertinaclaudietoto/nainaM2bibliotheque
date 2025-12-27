
using Microsoft.EntityFrameworkCore;

    public class GenreService :HandlerCRUD<Genre>
    {
        private readonly  ApplicationDbContext _context;

        public GenreService( ApplicationDbContext context): base(context, "Genres")
        {
            _context = context;
        }


    }