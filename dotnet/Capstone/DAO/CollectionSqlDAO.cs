using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone.Models;
using Capstone.Security;
using Capstone.Security.Models;

namespace Capstone.DAO
{
    public class CollectionSqlDAO : ICollectionDAO
    {
        private readonly string connectionString;

        public CollectionSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Collection> GetPublicCollections()
        {
            List<Collection> publicCollections = new List<Collection>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT collection_id, user_id, name, is_public FROM collections WHERE is_public = 1", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        publicCollections.Add(GetCollectionFromReader(reader));
                    }

                }
            }
            catch (SqlException)
            {
                throw;
            }
            return publicCollections;
        }

        public List<Collection> GetAllUserCollections(int userId)
        {
            List<Collection> userCollections = new List<Collection>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT collection_id, user_id, name, is_public FROM collections WHERE user_id = @user_id", conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        userCollections.Add(GetCollectionFromReader(reader));
                    }

                }
            }
            catch (SqlException)
            {
                throw;
            }
            return userCollections;
        }

        public void CreateCollection(Collection collection)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO collections (user_id, name, is_public)" +
                                                    "VALUES (@user_id, @name, 0);" +
                                                    "SELECT SCOPE_IDENTITY()", conn);
                    cmd.Parameters.AddWithValue("@user_id", collection.UserID);
                    cmd.Parameters.AddWithValue("@name", collection.Name);
                    collection.CollectionID = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public bool UpdateCollectionPrivacy(Collection collection, int privacyChange)
        {
            int output = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE collections " +
                                                    "SET is_public = @privacyChange, name = @name " +
                                                    "WHERE collection_id = @collection_id;", conn);
                    cmd.Parameters.AddWithValue("@collection_id", collection.CollectionID);
                    cmd.Parameters.AddWithValue("@privacyChange", privacyChange);
                    cmd.Parameters.AddWithValue("@name", collection.Name);
                    output = cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return (output == 1);
        }

        public Collection GetSingleCollection(int id)
        {
            Collection collection = new Collection();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT collection_id, user_id, name, is_public FROM collections WHERE collection_id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                      collection = GetCollectionFromReader(reader);
                    }

                }
            }
            catch (SqlException)
            {
                throw;
            }
            return collection;
        }

        /// <summary>
        /// Sends the SQL database a query to retrieve the number of ComicBooks in
        /// a collection with <paramref name="collectionId"/>.
        /// </summary>
        /// <param name="collectionId"></param>
        /// <returns>The count of comics in the collection.</returns>
        public int GetCountOfComicsInCollection(int collectionId)
        {
            int comicCount = -1;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT SUM(quantity) " +
                                                    "FROM collections_comics " +
                                                    "WHERE collection_id = @id; ", conn);
                    cmd.Parameters.AddWithValue("@id", collectionId);
                    comicCount = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return comicCount;
        }

        private Collection GetCollectionFromReader(SqlDataReader reader)
        {
            Collection c = new Collection()
            {
                CollectionID = Convert.ToInt32(reader["collection_id"]),
                UserID = Convert.ToInt32(reader["user_id"]),
                Name = Convert.ToString(reader["name"]),
                Public = Convert.ToBoolean(reader["is_public"]),
            };

            return c;
        }
    }
}
