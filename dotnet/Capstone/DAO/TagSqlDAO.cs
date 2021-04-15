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

        public Tag AddTagToDatabase(string description)
        {
            int tagId = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO tags (tag_description) " +
                                                    "VALUES(@description);" +
                                                    "SELECT SCOPE_IDENTITY();", conn);
                    cmd.Parameters.AddWithValue("@description", description);
                    tagId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (SqlException)
            {
                throw;
            }
            Tag tag = new Tag
            {
                Id = tagId,
                Description = description
            };
            return tag;
        }

        public bool LinkTagToComic(int comicId, int tagId)
        {
            int isSuccessful = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO comic_tags (tag_id, comic_id) " +
                                                    "VALUES(@tag_id, @comic_id);", conn);
                    cmd.Parameters.AddWithValue("@tag_id", tagId);
                    cmd.Parameters.AddWithValue("@comic_id", comicId);
                    isSuccessful = cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return (isSuccessful == 1);
        }

        public Tag GetTagByDescription(string description)
        {
            Tag tag = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT tag_id, tag_description " +
                                                    "FROM tags " +
                                                    "WHERE tag_description = @tag_description;", conn);
                    cmd.Parameters.AddWithValue("@tag_description", description);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
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

        public List<Tag> GetTagListForComicBook(int comicId)
        {
            List<Tag> tags = new List<Tag>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT t.tag_id, t.tag_description " +
                                                    "FROM tags t " +
                                                    "INNER JOIN comic_tags ct ON ct.tag_id = t.tag_id " +
                                                    "WHERE ct.comic_id = @comic_id;", conn);
                    cmd.Parameters.AddWithValue("@comic_id", comicId);
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

                    SqlCommand cmd = new SqlCommand("SELECT COUNT(quantity) " +
                                                    "FROM collections_comics cc " +
                                                    "INNER JOIN comics c ON c.comic_id = cc.comic_id " +
                                                    "INNER JOIN comic_tags ct ON ct.comic_id = c.comic_id " +
                                                    "WHERE ct.tag_id = @tag_id;", conn);
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
