using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone.DAO
{
    public class TagSqlDAO : ITagDAO
    {
        private readonly string connectionString;

        public TagSqlDAO (string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public bool AddTagToDatabase(string description)
        {
            int isSuccessful = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO tags (tag_description) " +
                                                    "VALUES(@description);", conn);
                    cmd.Parameters.AddWithValue("@description", description);
                    isSuccessful = cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return (isSuccessful == 1);
        }

        public bool LinkTagToComic(int comicId, int tagId)
        {
            int isSuccessful = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO tags (comic_id, tag_id) " +
                                                    "VALUES(@comic_id, @tag_id);", conn);
                    cmd.Parameters.AddWithValue("@comic_id", comicId);
                    cmd.Parameters.AddWithValue("@tag_id", tagId);
                    isSuccessful = cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return (isSuccessful == 1);
        }

        public bool DoesTagExist(int tagId)
        {
            int exists = -1;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT tag_id " +
                                                    "FROM tags " +
                                                    "WHERE tag_id = @tag_id;", conn);
                    cmd.Parameters.AddWithValue("@tag_id", tagId);
                    object result = cmd.ExecuteScalar();

                    if (Convert.IsDBNull(result))
                    {
                        exists = 0;
                    }
                    else
                    {
                        exists = Convert.ToInt32(result);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return (exists == 1);
        }

        public bool IsTagLinkedToComic(int comicId, int tagId)
        {
            int exists = -1;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT t.tag_id, ct.comic_id " +
                                                    "FROM tags t " +
                                                    "INNER JOIN comic_tags ct ON ct.tag_id = t.tag_id " +
                                                    "WHERE ct.comic_id = @comic_id AND ct.tag_id = @tag_id;", conn);
                    cmd.Parameters.AddWithValue("@comic_id", comicId);
                    cmd.Parameters.AddWithValue("@tag_id", tagId);
                    object result = cmd.ExecuteScalar();

                    if (Convert.IsDBNull(result))
                    {
                        exists = 0;
                    }
                    else
                    {
                        exists = Convert.ToInt32(result);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return (exists == 1);
        }

        public Tag GetTag(int tagId)
        {
            Tag tag = new Tag();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT tag_id " +
                                                    "FROM tags " +
                                                    "WHERE tag_id = @tag_id;", conn);
                    cmd.Parameters.AddWithValue("@tag_id", tagId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        tag = GetTagFromReader(reader);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return tag;
        }

        public List<Tag> GetAllTags()
        {
            List<Tag> tags = new List<Tag>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT tag_id, tag_description " +
                                                    "FROM tags;", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        tags.Add(GetTagFromReader(reader));
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return tags;
        }

        public int GetCountOfTagAcrossDatabase(int tagId)
        {
            int tagCount = -1;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT quantity " +
                                                    "FROM collections_comics cc " +
                                                    "INNER JOIN comics c ON c.comic_id = cc.comic_id " +
                                                    "INNER JOIN comic_tags ct ON ct.tag_id = t.tag_id " +
                                                    "WHERE t.tag_id = @tag_id;", conn);
                    cmd.Parameters.AddWithValue("@tag_id", tagId);
                    object result = cmd.ExecuteScalar();

                    if (Convert.IsDBNull(result))
                    {
                        tagCount = 0;
                    }
                    else
                    {
                        tagCount = Convert.ToInt32(result);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return tagCount;
        }

        private Tag GetTagFromReader(SqlDataReader reader)
        {
            Tag t = new Tag
            {
                Id = Convert.ToInt32(reader["tag_id"]),
                Description = Convert.ToString(reader["tag_description"])
            };
            return t;
        }
    }
}
