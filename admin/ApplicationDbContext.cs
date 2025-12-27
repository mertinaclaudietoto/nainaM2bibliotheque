using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Livre> Livres { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Auteur> Auteurs { get; set; }
    public DbSet<LivreDetails> LivreDetails { get; set; }
    public DbSet<LivreEmprunt> LivreEmprunt { get; set; }
    public DbSet<User> Users { get; set; }



    

}
