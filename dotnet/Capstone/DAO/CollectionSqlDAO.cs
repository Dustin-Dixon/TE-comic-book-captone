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

        public List<Collection> GetAllUserCollections(int userId)
        {
            List<Collection> userCollections = new List<Collection>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT collection_id, user_id, name FROM collections WHERE user_id = @user_id", conn);
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

        public Collection CreateCollection(int userId, string name)
        {
            int newCollectionId;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO collections (user_id, name) VALUES (@user_id, @name)", conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    cmd.Parameters.AddWithValue("@name", name);
                    newCollectionId = Convert.ToInt32(cmd.ExecuteScalar());

                }
            }
            catch (SqlException)
            {
                throw;
            }

            return ReturnAddedCollection(newCollectionId);
        }

        public Collection ReturnAddedCollection(int collectionId)
        {
            Collection collectionsFound = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT collection_id, user_id, name FROM collections WHERE collection_id = @collection_id", conn);
                    cmd.Parameters.AddWithValue("@collection_id", collectionId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        collectionsFound = GetCollectionFromReader(reader);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return collectionsFound;
        }

        private Collection GetCollectionFromReader(SqlDataReader reader)
        {
            Collection c = new Collection()
            {
                CollectionID = Convert.ToInt32(reader["collection_id"]),
                UserID = Convert.ToInt32(reader["user_id"]),
                Name = Convert.ToString(reader["name"]),
            };

            return c;
        }
    }
}
