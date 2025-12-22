using Microsoft.Data.SqlClient;
using System.Data;
namespace Client.Repositorys;
public class LivreRepository
{
    private readonly string _connectionString;

    public LivreRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // 1️⃣ Récupérer tous les livres
    public List<LivreDetails> GetAll()
    {
        var livres = new List<LivreDetails>();

        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("SELECT * FROM vw_LivreDetails ORDER BY LivreNom", conn);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    livres.Add(MapLivre(reader));
                }
            }
        }

        return livres;
    }

    // 2️⃣ Récupérer les livres par idgenre
    public List<LivreDetails> GetByGenre(int idGenre)
    {
        var livres = new List<LivreDetails>();

        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("SELECT * FROM vw_LivreDetails WHERE GenreId=@idGenre ORDER BY LivreNom", conn);
            cmd.Parameters.AddWithValue("@idGenre", idGenre);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    livres.Add(MapLivre(reader));
                }
            }
        }

        return livres;
    }

    // 3️⃣ Pagination (avec ou sans filtre)
    public List<LivreDetails> GetPaged(int pageNumber, int pageSize, int? idGenre = null)
    {
        var livres = new List<LivreDetails>();
        int offset = (pageNumber - 1) * pageSize;

        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();

            string sql = @"
                SELECT * FROM vw_LivreDetails
                /**WHERE_CONDITION**/
                ORDER BY LivreNom
                OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";

            if (idGenre.HasValue)
            {
                sql = sql.Replace("/**WHERE_CONDITION**/", "WHERE GenreId=@idGenre");
            }
            else
            {
                sql = sql.Replace("/**WHERE_CONDITION**/", "");
            }

            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@offset", offset);
            cmd.Parameters.AddWithValue("@pageSize", pageSize);
            if (idGenre.HasValue)
            {
                cmd.Parameters.AddWithValue("@idGenre", idGenre.Value);
            }

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    livres.Add(MapLivre(reader));
                }
            }
        }

        return livres;
    }

    // Méthode privée pour mapper le reader vers le modèle
    private LivreDetails MapLivre(SqlDataReader reader)
    {
        return new LivreDetails
        {
            LivreId = reader.GetInt32(reader.GetOrdinal("LivreId")),
            LivreNom = reader.GetString(reader.GetOrdinal("LivreNom")),
            LivrePhoto = reader.IsDBNull(reader.GetOrdinal("LivrePhoto")) ? "" : reader.GetString(reader.GetOrdinal("LivrePhoto")),
            DateEntree = reader.GetDateTime(reader.GetOrdinal("DateEntree")),
            DateEdition = reader.IsDBNull(reader.GetOrdinal("DateEdition")) ? null : reader.GetDateTime(reader.GetOrdinal("DateEdition")),
            GenreId = reader.GetInt32(reader.GetOrdinal("GenreId")),
            GenreNom = reader.GetString(reader.GetOrdinal("GenreNom")),
            AuteurId = reader.GetInt32(reader.GetOrdinal("AuteurId")),
            AuteurNom = reader.GetString(reader.GetOrdinal("AuteurNom"))
        };
    }
}
