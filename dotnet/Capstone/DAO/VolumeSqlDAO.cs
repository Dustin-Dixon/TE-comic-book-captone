using Capstone.Models;
using System;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public class VolumeSqlDAO : IVolumeDAO
    {
        private string connectionString;

        public VolumeSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool AddVolume(Volume newVolume)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO volumes (volume_id, name, publisher, api_detail_url, site_detail_url) " +
                        "VALUES " +
                        "(@volumeId, @name, @publisher, @apiDetailUrl, @siteDetailUrl); " +
                        "INSERT INTO volume_images (volume_id, icon_url, small_url, medium_url, thumb_url) " +
                        "VALUES " +
                        "(@volumeId, @iconURL, @smallURL, @mediumURL, @thumbURL);",
                        conn);
                    cmd.Parameters.AddWithValue("@volumeId", newVolume.Id);
                    cmd.Parameters.AddWithValue("@name", newVolume.Name);
                    cmd.Parameters.AddWithValue("@publisher", newVolume.Publisher.Name);
                    cmd.Parameters.AddWithValue("@apiDetailUrl", newVolume.ApiDetailUrl);
                    cmd.Parameters.AddWithValue("@siteDetailUrl", newVolume.SiteDetailUrl);
                    cmd.Parameters.AddWithValue("@iconURL", newVolume.Image.IconUrl);
                    cmd.Parameters.AddWithValue("@smallURL", newVolume.Image.SmallUrl);
                    cmd.Parameters.AddWithValue("@mediumURL", newVolume.Image.MediumUrl);
                    cmd.Parameters.AddWithValue("@thumbURL", newVolume.Image.ThumbUrl);

                    if (cmd.ExecuteNonQuery() == 2)
                    {
                        newVolume.Image.VolumeId = newVolume.Id;
                        return true;
                    }
                    return false;
                }
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public Volume GetById(int volumeId)
        {
            Volume volume = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(
                        "SELECT v.volume_id, v.name, v.publisher, v.site_detail_url, i.icon_url, i.small_url, i.medium_url, i.thumb_url " +
                        "FROM volumes v " +
                        "JOIN volume_images i ON v.volume_id = i.volume_id " +
                        "WHERE v.volume_id = @volume_id",
                        conn);
                    cmd.Parameters.AddWithValue("@volume_id", volumeId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        volume = GetVolumeFromReader(reader);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return volume;
        }

        public Volume GetComicVolume(int comicId)
        {
            Volume volume = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(
                        "SELECT v.volume_id, v.name, v.publisher, v.api_detail_url, v.site_detail_url, i.icon_url, i.small_url, i.medium_url, i.thumb_url " +
                        "FROM volumes v " +
                        "JOIN volume_images i ON v.volume_id = i.volume_id " +
                        "JOIN comics c ON c.volume_id = v.volume_id " +
                        "WHERE c.comic_id = @comicId",
                        conn);
                    cmd.Parameters.AddWithValue("@comicId", comicId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        volume = GetVolumeFromReader(reader);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return volume;
        }

        private Volume GetVolumeFromReader(SqlDataReader reader)
        {
            return new Volume()
            {
                Id = Convert.ToInt32(reader["volume_id"]),
                Name = Convert.ToString(reader["name"]),
                Publisher = new Publisher()
                {
                    Name = Convert.ToString(reader["publisher"]),
                },
                ApiDetailUrl = Convert.ToString(reader["api_detail_url"]),
                SiteDetailUrl = Convert.ToString(reader["site_detail_url"]),
                Image = new VolumeImages()
                {
                    VolumeId = Convert.ToInt32(reader["volume_id"]),
                    IconUrl = Convert.ToString(reader["icon_url"]),
                    SmallUrl = Convert.ToString(reader["small_url"]),
                    MediumUrl = Convert.ToString(reader["medium_url"]),
                    ThumbUrl = Convert.ToString(reader["thumb_url"])
                }
            };
        }
    }
}
