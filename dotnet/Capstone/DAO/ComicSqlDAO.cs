using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.DAO
{
    public class ComicSqlDAO : IComicDAO
    {
        private readonly string connectionString;

        public ComicSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public ComicBook AddComicToCollection(int collectionId)
        {
            throw new NotImplementedException();
        }

        public List<ComicBook> ComicsInCollection(int userId, Collection collection)
        {
            List<ComicBook> comicsInCollection = new List<ComicBook>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT com.comic_id, com.name, com.author, com.release_date" +
                                                    "FROM comics com" +
                                                    "INNER JOIN collections_comics cc ON com.comic_id = cc.comic_id" +
                                                    "INNER JOIN collections col ON cc.collection_id = col.collection_id" +
                                                    "WHERE col.user_id = @user_id AND col.collection_id = @collection_id", conn);
                    cmd.Parameters.AddWithValue("@user_id", collection.UserID);
                    cmd.Parameters.AddWithValue("@collection_id", collection.CollectionID);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        comicsInCollection.Add(GetComicFromReader(reader));
                    }

                }
            }
            catch (SqlException)
            {
                throw;
            }
            return comicsInCollection;
        }

        private ComicBook GetComicFromReader(SqlDataReader reader)
        {
            ComicBook cb = new ComicBook()
            {
                ComicID = Convert.ToInt32(reader["comic_id"]),
                Name = Convert.ToString(reader["name"]),
                Author = Convert.ToString(reader["author"]),
                ReleaseDate = Convert.ToDateTime(reader["release_date"])
            };

            return cb;
        }
    }
}
