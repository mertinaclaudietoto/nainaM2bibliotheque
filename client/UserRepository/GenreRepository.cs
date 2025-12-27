using Microsoft.Data.SqlClient;  // ✅ Correct pour .NET Core/5/6/7
using System.Data;  
namespace Client.Repositorys;
public class GenreRepository
{
    private readonly string _connectionString;

    public GenreRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // 1️⃣ Récupérer tous les genres
    public List<Genre> GetAll()
    {
        var genres = new List<Genre>();

        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("SELECT id, nom FROM genre ORDER BY nom", conn);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    genres.Add(new Genre
                    {
                        Id = reader.GetInt32(0),
                        Nom = reader.GetString(1)
                    });
                }
            }
        }

        return genres;
    }

    // 2️⃣ Récupérer un genre par id
    public Genre? GetById(int id)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("SELECT id, nom FROM genre WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Genre
                    {
                        Id = reader.GetInt32(0),
                        Nom = reader.GetString(1)
                    };
                }
            }
        }

        return null;
    }

    // 3️⃣ Ajouter un nouveau genre
    public int Add(string nom)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("INSERT INTO genre (nom) VALUES (@nom); SELECT SCOPE_IDENTITY();", conn);
            cmd.Parameters.AddWithValue("@nom", nom);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }
    }

    // 4️⃣ Mettre à jour un genre
    public bool Update(int id, string nom)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("UPDATE genre SET nom=@nom WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("@nom", nom);
            cmd.Parameters.AddWithValue("@id", id);

            return cmd.ExecuteNonQuery() > 0;
        }
    }

    // 5️⃣ Supprimer un genre
    public bool Delete(int id)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("DELETE FROM genre WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
