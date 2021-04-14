using Capstone.Models;
using Microsoft.AspNetCore.Mvc;
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
            int isSuccessful = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO collections_comics (collection_id, comic_id, quantity) " +
                                                    "VALUES(@collection_id, @comic_id, @quantity)", conn);
                    cmd.Parameters.AddWithValue("@collection_id", collectionId);
                    cmd.Parameters.AddWithValue("@comic_id", comicBook.Id);
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
        public bool DeleteComicFromCollection(int collectionId, int comicId)
        {
            int isSuccessful = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("DELETE " +
                                                    "FROM collections_comics " +
                                                    "WHERE collection_id = @collection_id AND comic_id = @comic_id; ", conn);
                    cmd.Parameters.AddWithValue("@collection_id", collectionId);
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

        public int GetComicQuantityInCollection(int collectionId, int comicId)
        {
            int quantity = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT quantity " +
                                                    "FROM collections_comics " +
                                                    "WHERE collection_id = @collection_id AND comic_id = @comic_id;", conn);
                    cmd.Parameters.AddWithValue("@collection_id", collectionId);
                    cmd.Parameters.AddWithValue("@comic_id", comicId);

                    object result = cmd.ExecuteScalar();
                    if (Convert.IsDBNull(result))
                    {
                        quantity = 0;
                    } else
                    {
                        quantity = Convert.ToInt32(result);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return quantity;
        }

        public bool UpdateQuantityOfComicInCollection(int collectionId, int comicId, int quantity)
        {
            int isSuccessful = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE collections_comics " +
                                                    "SET quantity = @quantity " +
                                                    "WHERE collection_id = @collection_id AND comic_id = @comic_id;", conn);
                    cmd.Parameters.AddWithValue("@collection_id", collectionId);
                    cmd.Parameters.AddWithValue("@comic_id", comicId);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
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
        /// Adds <paramref name="comicBook"/> to the comics table in the SQL database.
        /// </summary>
        /// <param name="comicBook"></param>
        public bool AddComic(ComicBook comicBook)
        {
            int isSuccessful = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO comics (comic_id, name, issue_number, cover_date, site_detail_url, api_detail_url) " +
                                                    "VALUES (@comic_id, @name, @issue_number, @cover_date, @site_detail_url, @api_detail_url)", conn);
                    cmd.Parameters.AddWithValue("@comic_id", comicBook.Id);
                    cmd.Parameters.AddWithValue("@name", comicBook.Name);
                    cmd.Parameters.AddWithValue("@issue_number", comicBook.IssueNumber);
                    cmd.Parameters.AddWithValue("@cover_date", comicBook.CoverDate);
                    cmd.Parameters.AddWithValue("@site_detail_url", comicBook.SiteDetailUrl);
                    cmd.Parameters.AddWithValue("@api_detail_url", comicBook.ApiDetailUrl);
                    isSuccessful = cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return (isSuccessful == 1);
        }

        public bool AddImages(ComicBook comicBook)
        {
            int isSuccessful = 0;
            comicBook.Image.ComicId = comicBook.Id;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO comic_images (comic_id, icon_url, small_url, medium_url, thumb_url) " +
                                                    "VALUES(@comic_id, @icon_url, @small_url, @medium_url, @thumb_url);", conn);
                    cmd.Parameters.AddWithValue("@comic_id", comicBook.Id);
                    cmd.Parameters.AddWithValue("@icon_url", comicBook.Image.IconUrl);
                    cmd.Parameters.AddWithValue("@small_url", comicBook.Image.SmallUrl);
                    cmd.Parameters.AddWithValue("@medium_url", comicBook.Image.MediumUrl);
                    cmd.Parameters.AddWithValue("@thumb_url", comicBook.Image.ThumbUrl);
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

                    SqlCommand cmd = new SqlCommand("SELECT com.comic_id, com.name, com.issue_number, com.cover_date, com.site_detail_url, " +
                                                    "com.api_detail_url, ci.icon_url, ci.small_url, ci.medium_url, ci.thumb_url " +
                                                    "FROM comics com " +
                                                    "INNER JOIN comic_images ci ON ci.comic_id = com.comic_id " +
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

        public List<ComicBook> LocalComicSearch(string searchTerm)
        {
            List<ComicBook> searchList = new List<ComicBook>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT TOP (50) com.comic_id, com.name, com.issue_number, com.cover_date, " +
                                                    "com.site_detail_url, com.api_detail_url, ci.icon_url, ci.small_url, ci.medium_url, ci.thumb_url " +
                                                    "FROM comics com " +
                                                    "INNER JOIN comic_images ci ON ci.comic_id = com.comic_id " +
                                                    "WHERE name LIKE @searchTerm OR issue_number LIKE @searchTerm " +
                                                    "OR cover_date LIKE @searchTerm; ", conn);
                    cmd.Parameters.AddWithValue("@searchTerm", $"%{searchTerm}%");
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        searchList.Add(GetComicFromReader(reader));
                    }

                }
            }
            catch (SqlException)
            {
                throw;
            }
            return searchList;
        }

        public ComicBook GetById(int comicId)
        {
            ComicBook returnComic = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT com.comic_id, com.name, com.issue_number, com.cover_date, " +
                                                    "com.site_detail_url, com.api_detail_url, ci.icon_url, ci.small_url, ci.medium_url, ci.thumb_url " +
                                                    "FROM comics com " +
                                                    "INNER JOIN comic_images ci ON ci.comic_id = com.comic_id " +
                                                    "WHERE com.comic_id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", comicId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        returnComic = GetComicFromReader(reader);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return returnComic;
        }

        private ComicBook GetComicFromReader(SqlDataReader reader)
        {
            ComicBook cb = new ComicBook()
            {
                Id = Convert.ToInt32(reader["comic_id"]),
                Name = Convert.ToString(reader["name"]),
                IssueNumber = Convert.ToString(reader["issue_number"]),
                CoverDate = Convert.ToString(reader["cover_date"]),
                SiteDetailUrl = Convert.ToString(reader["site_detail_url"]),
                ApiDetailUrl = Convert.ToString(reader["api_detail_url"]),
                Image = new ComicImages
                {
                    ComicId = Convert.ToInt32(reader["comic_id"]),
                    IconUrl = Convert.ToString(reader["icon_url"]),
                    SmallUrl = Convert.ToString(reader["small_url"]),
                    MediumUrl = Convert.ToString(reader["medium_url"]),
                    ThumbUrl = Convert.ToString(reader["thumb_url"])
                },
                Characters = new List<Character>()
            };

            return cb;
        }
    }
}
