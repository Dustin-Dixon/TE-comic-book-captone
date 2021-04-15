using Capstone.Models;
using Capstone.Models.Stats;
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

                        SqlCommand cmd = new SqlCommand("SELECT COUNT(creator_id) " +
                                                        "FROM comic_creators " +
                                                        "WHERE creator_id = @creator_id;", conn);
                        cmd.Parameters.AddWithValue("@creator_id", creators[i].Id);
                        isFound = Convert.ToInt32(cmd.ExecuteScalar());
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

        public List<Creator> GetComicCreators(int comicId)
        {
            List<Creator> creators = new List<Creator>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(
                        "SELECT c.creator_id, c.name FROM comic_creators c " +
                        "JOIN comic_creators_contributions co ON co.creator_id = c.creator_id " +
                        "WHERE co.comic_id = @comic_id; ", conn);
                    cmd.Parameters.AddWithValue("@comic_id", comicId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        creators.Add(GetCreatorFromReader(reader));
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return creators;
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
        public List<CreatorCount> GetCollectionCreatorCount(int collectionId)
        {
            List<CreatorCount> creatorStats = new List<CreatorCount>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT cre.creator_id, cre.name, COUNT(cre.creator_id) AS count from comics com " +
                                   "JOIN collections_comics col on com.comic_id = col.comic_id " +
                                   "JOIN comic_creators_contributions cre_con on com.comic_id = cre_con.comic_id " +
                                   "JOIN comic_creators cre ON cre_con.creator_id = cre.creator_id " +
                                   "WHERE col.collection_id = @collectionId " +
                                   "GROUP BY cre.creator_id, cre.name " +
                                   "ORDER BY count DESC;";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@collectionId", collectionId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        creatorStats.Add(new CreatorCount()
                        {
                            Creator = GetCreatorFromReader(reader),
                            Count = Convert.ToInt32(reader["count"])
                        });
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return creatorStats;
        }
        public List<CreatorCount> GetTotalCollectionCreatorCount()
        {
            List<CreatorCount> creatorStats = new List<CreatorCount>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT cre.creator_id, cre.name, COUNT(cre.creator_id) AS count from comics com " +
                                   "JOIN collections_comics col on com.comic_id = col.comic_id " +
                                   "JOIN comic_creators_contributions cre_con on com.comic_id = cre_con.comic_id " +
                                   "JOIN comic_creators cre ON cre_con.creator_id = cre.creator_id " +
                                   "GROUP BY cre.creator_id, cre.name " +
                                   "ORDER BY count DESC;";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        creatorStats.Add(new CreatorCount()
                        {
                            Creator = GetCreatorFromReader(reader),
                            Count = Convert.ToInt32(reader["count"])
                        });
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return creatorStats;
        }

    }
}
