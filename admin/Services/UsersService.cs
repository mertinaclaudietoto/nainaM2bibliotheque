
using Microsoft.EntityFrameworkCore;

    public class UsersService :HandlerCRUD<User>
    {
        private readonly  ApplicationDbContext _context;

        public UsersService( ApplicationDbContext context): base(context, "Users")
        {
            _context = context;
        }
        public async Task<User> GetById(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
            public List<StatAge> GetPourcentageMembreParAge()
        {
            var today = DateTime.Today;

            // Membres avec date de naissance connue
            var membres = _context.Users
                .Where(u => u.DateDeNaissance.HasValue)
                .Select(u => new
                {
                    Age = today.Year - u.DateDeNaissance.Value.Year
                        - (u.DateDeNaissance.Value.Date > today.AddYears(
                                -(today.Year - u.DateDeNaissance.Value.Year)) ? 1 : 0)
                })
                .ToList();

            int total = membres.Count;

            if (total == 0)
                return new List<StatAge>();
            var result = membres
                .GroupBy(m =>
                    m.Age < 18 ? "0 - 17 ans" :
                    m.Age <= 25 ? "18 - 25 ans" :
                    m.Age <= 35 ? "26 - 35 ans" :
                    m.Age <= 50 ? "36 - 50 ans" :
                                "51 ans et plus"
                )
                .Select(g => new StatAge
                {
                    trancheAge = g.Key,
                    pourcentage = Math.Round(
                        (double)g.Count() * 100 / total, 2
                    )
                })
                .OrderByDescending(s => s.pourcentage)
                .ToList();

            return result;
        }
        
}