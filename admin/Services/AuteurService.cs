
using Microsoft.EntityFrameworkCore;

    public class AuteurService :HandlerCRUD<Auteur>
    {
        private readonly  ApplicationDbContext _context;

        public AuteurService( ApplicationDbContext context): base(context, "Auteurs")
        {
            _context = context;
        }


    }