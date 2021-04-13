using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.DAO
{
    public class CreatorSqlDAO : ICreatorDAO
    {
        private readonly string connectionString;

        public CreatorSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

       public bool AddCreatorCreditToTable(Creator creator)
        {
            int isSuccessful = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO comic_creators " +
                                                    "VALUES(@creator_id, @name);", conn);
                    cmd.Parameters.AddWithValue("@creator_id", creator.Id);
                    cmd.Parameters.AddWithValue("@name", creator.Name);
                    isSuccessful = cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return (isSuccessful == 1);
        }

        public bool LinkCreatorToComic(int creatorId, int comicId)
        {
            int isSuccessful = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO comic_creators_contributions (comic_id, creator_id) " +
                                                    "VALUES(@comic_id, @creator_id);", conn);
                    cmd.Parameters.AddWithValue("@comic_id", comicId);
                    cmd.Parameters.AddWithValue("@creator_id", creatorId);
                    isSuccessful = cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return (isSuccessful == 1);
        }

        public void CheckDatabaseForCreators(List<Creator> creators)
        {
            for (int i = 0; i < creators.Count; i++)
            {
                int isFound = 0;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand("SELECT creator_id " +
                                                        "FROM comic_creators " +
                                                        "WHERE creator_id = @creator_id;", conn);
                        cmd.Parameters.AddWithValue("@creator_id", creators[i].Id);
                        isFound = cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException)
                {
                    throw;
                }
                if (isFound == 1)
                {
                    creators[i].InDatabase = true;
                }
            }
        }

        private Creator GetCreatorFromReader(SqlDataReader reader)
        {
            Creator c = new Creator
            {
                Id = Convert.ToInt32(reader["creator_id"]),
                Name = Convert.ToString(reader["name"])
            };
            return c;
        }
    }


    
}
