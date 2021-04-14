using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

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

                    SqlCommand cmd = new SqlCommand("SELECT c.collection_id, c.user_id, u.username, c.name, c.is_public " +
                                                    "FROM collections c " +
                                                    "JOIN users u ON c.user_id = u.user_id " +
                                                    "WHERE is_public = 1", conn);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Collection collection = GetCollectionFromReader(reader);
                        collection.ComicCount = GetCountOfComicsInCollection(collection.CollectionID);
                        publicCollections.Add(collection);
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


                    SqlCommand cmd = new SqlCommand("SELECT c.collection_id, c.user_id, u.username, c.name, c.is_public " +
                                                    "FROM collections c " +
                                                    "JOIN users u ON c.user_id = u.user_id " +
                                                    "WHERE c.user_id = @user_id", conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Collection collection = GetCollectionFromReader(reader);
                        collection.ComicCount = GetCountOfComicsInCollection(collection.CollectionID);
                        userCollections.Add(collection);
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

                    SqlCommand cmd = new SqlCommand("SELECT c.collection_id, c.user_id, u.username, c.name, c.is_public " +
                                                    "FROM collections c " +
                                                    "JOIN users u ON c.user_id = u.user_id " +
                                                    "WHERE c.collection_id = @id", conn);

                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        collection = GetCollectionFromReader(reader);
                        collection.CollectionID = GetCountOfComicsInCollection(id);
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

                    object result = cmd.ExecuteScalar();
                    if (Convert.IsDBNull(result))
                    {
                        comicCount = 0;
                    }
                    else
                    {
                        comicCount = Convert.ToInt32(result);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return comicCount;
        }

        public int GetCountOfComicsInAllCollections()
        {
            int comicCount = -1;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT SUM(quantity) as NumberOfComicsInAllCollections " +
                                                    "FROM collections_comics; ", conn);

                    object result = cmd.ExecuteScalar();
                    if (Convert.IsDBNull(result))
                    {
                        comicCount = 0;
                    }
                    else
                    {
                        comicCount = Convert.ToInt32(result);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return comicCount;
        }

        /// <summary>
        /// Queries the SQL database to retrieve the total number of ComicBooks across
        /// all collections for a user with <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Count of all comics across all of the user's collections.</returns>
        public int UserTotalComicCount(int userId)
        {
            int comicTotal = -1;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT SUM(quantity) as TotalNumberOfComics " +
                                                    "FROM collections_comics " +
                                                    "JOIN collections ON collections.collection_id = collections_comics.collection_id " +
                                                    "WHERE user_id = @user_id; ", conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);

                    object result = cmd.ExecuteScalar();
                    if (Convert.IsDBNull(result))
                    {
                        comicTotal = 0;
                    }
                    else
                    {
                        comicTotal = Convert.ToInt32(result);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return comicTotal;
        }

        public bool DeleteCollection(int collectionId)
        {
            int isSuccessful = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("DELETE " +
                                                    "FROM collections " +
                                                    "WHERE collection_id = @collection_id", conn);
                    cmd.Parameters.AddWithValue("@collection_id", collectionId);
                    isSuccessful = Convert.ToInt32(cmd.ExecuteNonQuery());
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return (isSuccessful == 1);
        }

        public bool DeleteCollectionFromComicLinker(int collectionId)
        {
            int isSuccessful = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("DELETE " +
                                                    "FROM collections_comics " +
                                                    "WHERE collection_id = @collection_id", conn);
                    cmd.Parameters.AddWithValue("@collection_id", collectionId);
                    isSuccessful = Convert.ToInt32(cmd.ExecuteNonQuery());
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return (isSuccessful > 0);
        }

        private Collection GetCollectionFromReader(SqlDataReader reader)
        {
            Collection c = new Collection()
            {
                CollectionID = Convert.ToInt32(reader["collection_id"]),
                UserID = Convert.ToInt32(reader["user_id"]),
                Username = Convert.ToString(reader["username"]),
                Name = Convert.ToString(reader["name"]),
                Public = Convert.ToBoolean(reader["is_public"]),
            };

            return c;
        }
    }
}
