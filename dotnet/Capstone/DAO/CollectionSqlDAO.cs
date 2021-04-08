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
