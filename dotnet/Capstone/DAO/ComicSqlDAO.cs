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

        public void AddComicToCollection(int collectionId, ComicBook comicBook)
        {
            try
            {
                AddComicToComicTable(comicBook);
                AddComicToCollectionsComicsTable(collectionId, comicBook.ComicID);
            }
            catch (SqlException)
            {
                throw;
            }
        }

        /// <summary>
        /// Adds <paramref name="comicBook"/> to the comics table in the SQL database.
        /// </summary>
        /// <param name="comicBook"></param>
        private void AddComicToComicTable(ComicBook comicBook)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO comics (name, author, release_date) " +
                                                    "VALUES (@name, @author, @release_date); " +
                                                    "SELECT SCOPE_IDENTITY()", conn);
                    cmd.Parameters.AddWithValue("@name", comicBook.Name);
                    cmd.Parameters.AddWithValue("@author", comicBook.Author);
                    cmd.Parameters.AddWithValue("@release_date", comicBook.ReleaseDate);
                    comicBook.ComicID = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (SqlException)
            {
                throw;
            }
        }

        /// <summary>
        /// Adds a ComicBook with <paramref name="comicId"/> to a Collection 
        /// with <paramref name="collectionId"/> in the collections_comics table 
        /// in the SQL database.
        /// </summary>
        /// <param name="collectionId"></param>
        /// <param name="comicId"></param>
        private void AddComicToCollectionsComicsTable(int collectionId, int comicId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO collections_comics (collection_id, comic_id, quantity) " +
                                                    "VALUES(@collection_id, @comic_id, @quantity)", conn);
                    cmd.Parameters.AddWithValue("@collection_id", collectionId);
                    cmd.Parameters.AddWithValue("@comic_id", comicId);
                    cmd.Parameters.AddWithValue("@quantity", 1);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves a list of ComicBook objects from the SQL database that are in 
        /// the collection with <paramref name="collectionId"/>.
        /// </summary>
        /// <param name="collectionId"></param>
        /// <returns>A list of ComicBook objects in the collection.</returns>
        public List<ComicBook> ComicsInCollection(int collectionId)
        {
            List<ComicBook> comicsInCollection = new List<ComicBook>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT com.comic_id, com.name, com.author, com.release_date " +
                                                    "FROM comics com " +
                                                    "INNER JOIN collections_comics cc ON com.comic_id = cc.comic_id " +
                                                    "INNER JOIN collections col ON cc.collection_id = col.collection_id " +
                                                    "WHERE col.collection_id = @collection_id", conn);
                    cmd.Parameters.AddWithValue("@collection_id", collectionId);
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
