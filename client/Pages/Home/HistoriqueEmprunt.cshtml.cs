using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

public class HistoriqueEmpruntModel : PageModel
{
    private readonly IConfiguration _configuration;

    public HistoriqueEmpruntModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public List<LivreEmpruntVM> Emprunts { get; set; } = new();

    public IActionResult OnGet()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
        {
            return RedirectToPage("/Login");
        }

        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();

            var cmd = new SqlCommand(@"
                SELECT *
                FROM v_livre_emprunt
                WHERE UtilisateurId = @userId
                ORDER BY dateemprunt DESC
            ", conn);

            cmd.Parameters.AddWithValue("@userId", userId.Value);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Emprunts.Add(new LivreEmpruntVM
                    {
                        LivreId = reader.GetInt32(reader.GetOrdinal("LivreId")),
                        LivreNom = reader["LivreNom"].ToString(),
                        LivrePhoto = reader["LivrePhoto"].ToString(),
                        AuteurNom = reader["AuteurNom"].ToString(),
                        GenreNom = reader["GenreNom"].ToString(),
                        DateEmprunt = reader["dateemprunt"] as DateTime?,
                        DateLimite = reader["datelimite"] as DateTime?,
                        DateRetour = reader["dateretouredelivre"] as DateTime?
                    });
                }
            }
        }

        return Page();
    }
}
