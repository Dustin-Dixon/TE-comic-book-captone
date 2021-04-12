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

        public bool AddComicToCollection(int collectionId, ComicBook comicBook)
        {
            bool addToComics;
            bool addToCollectionsComics;
            try
            {
                addToComics = AddComicToComicTable(comicBook);
                addToCollectionsComics = AddComicToCollectionsComicsTable(collectionId, comicBook.Id);
            }
            catch (SqlException)
            {
                throw;
            }
            return (addToComics && addToCollectionsComics);
        }

        /// <summary>
        /// Adds <paramref name="comicBook"/> to the comics table in the SQL database.
        /// </summary>
        /// <param name="comicBook"></param>
        private bool AddComicToComicTable(ComicBook comicBook)
        {
            int isSuccessful = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO comics (comic_id, name, issue_number, cover_date, detail_url) " +
                                                    "VALUES (@comic_id, @name, @issue_number, @cover_date, @detail_url); " +
                                                    "SELECT SCOPE_IDENTITY()", conn);
                    cmd.Parameters.AddWithValue("@comic_id", comicBook.Id);
                    cmd.Parameters.AddWithValue("@name", comicBook.Name);
                    cmd.Parameters.AddWithValue("@issue_number", comicBook.IssueNumber);
                    cmd.Parameters.AddWithValue("@cover_date", comicBook.CoverDate);
                    cmd.Parameters.AddWithValue("@detail_url", comicBook.SiteDetailUrl);
                    isSuccessful = cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return (isSuccessful == 1);
        }

        /// <summary>
        /// Adds a ComicBook with <paramref name="comicId"/> to a Collection 
        /// with <paramref name="collectionId"/> in the collections_comics table 
        /// in the SQL database.
        /// </summary>
        /// <param name="collectionId"></param>
        /// <param name="comicId"></param>
        private bool AddComicToCollectionsComicsTable(int collectionId, int comicId)
        {
            int isSuccessful = 0;
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
                    isSuccessful = cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return (isSuccessful == 1);
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

                    SqlCommand cmd = new SqlCommand("SELECT com.comic_id, com.name, com.issue_number, com.cover_date, com.detail_url " +
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
                Id = Convert.ToInt32(reader["comic_id"]),
                Name = Convert.ToString(reader["name"]),
                IssueNumber = Convert.ToString(reader["issue_number"]),
                CoverDate = Convert.ToString(reader["cover_date"]),
                SiteDetailUrl = Convert.ToString(reader["detail_url"])
            };

            return cb;
        }
    }
}
