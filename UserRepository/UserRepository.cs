using Microsoft.Data.SqlClient;  // ✅ Correct pour .NET Core/5/6/7
using System.Data;               // Pour DataTable, IDbConnection si besoin

namespace Client.Repositorys;
public class UserRepository
{
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Vérifier si login existe
    public bool LoginExists(string login)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("SELECT COUNT(*) FROM users WHERE login=@login", conn);
            cmd.Parameters.AddWithValue("@login", login);
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }
    }

    // Insérer un utilisateur
    public void Register(string nom, string prenom, string email, string login, string motdepasse, DateTime? dateNaissance)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand(@"INSERT INTO users
                (nom, prenom, email, login, motdepasse, datedenaissance)
                VALUES (@nom, @prenom, @email, @login, @motdepasse, @datedenaissance)", conn);

            cmd.Parameters.AddWithValue("@nom", nom);
            cmd.Parameters.AddWithValue("@prenom", prenom);
            cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(email) ? DBNull.Value : email);
            cmd.Parameters.AddWithValue("@login", login);
            cmd.Parameters.AddWithValue("@motdepasse", motdepasse); // ⚠️ hacher avant production
            cmd.Parameters.AddWithValue("@datedenaissance", dateNaissance.HasValue ? (object)dateNaissance.Value : DBNull.Value);

            cmd.ExecuteNonQuery();
        }
    }

    // Vérifier login + motdepasse
    // public bool ValidateUser(string login, string motdepasse)
    // {
    //     using (var conn = new SqlConnection(_connectionString))
    //     {
    //         conn.Open();
    //         var cmd = new SqlCommand("SELECT COUNT(*) FROM users WHERE login=@login AND motdepasse=@motdepasse", conn);
    //         cmd.Parameters.AddWithValue("@login", login);
    //         cmd.Parameters.AddWithValue("@motdepasse", motdepasse);
    //         int count = (int)cmd.ExecuteScalar();
    //         return count > 0;
    //     }
    // }
            public int? ValidateUser(string login, string motdepasse)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand(
                    "SELECT id FROM users WHERE login=@login AND motdepasse=@motdepasse",
                    conn
                );

                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@motdepasse", motdepasse);

                var result = cmd.ExecuteScalar();

                if (result != null)
                    return Convert.ToInt32(result);

                return null;
            }
        }

}
