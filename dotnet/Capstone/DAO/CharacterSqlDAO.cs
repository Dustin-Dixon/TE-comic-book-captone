using Capstone.Models;
using Capstone.Models.Stats;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone.DAO
{
    public class CharacterSqlDAO : ICharacterDAO
    {
        private readonly string connectionString;

        public CharacterSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Character GetCharacterById(int charId)
        {
            Character character = new Character();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT character_id, name " +
                                                    "FROM characters " +
                                                    "WHERE character_id = @character_id;", conn);
                    cmd.Parameters.AddWithValue("@character_id", charId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        character = GetCharacterFromReader(reader);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return character;
        }

        public bool AddCharacterToTable(Character character)
        {
            int isSuccessful = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO characters (character_id, name) " +
                                                    "VALUES(@character_id, @name);", conn);
                    cmd.Parameters.AddWithValue("@character_id", character.Id);
                    cmd.Parameters.AddWithValue("@name", character.Name);
                    isSuccessful = cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return (isSuccessful == 1);
        }

        public bool LinkCharacterToComic(int charId, int comicId)
        {
            int isSuccessful = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO comic_characters (comic_id, character_id) " +
                                                    "VALUES(@comic_id, @character_id);", conn);
                    cmd.Parameters.AddWithValue("@comic_id", comicId);
                    cmd.Parameters.AddWithValue("@character_id", charId);
                    isSuccessful = cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return (isSuccessful == 1);
        }

        public void CheckDatabaseForCharacters(List<Character> characters)
        {
            for (int i = 0; i < characters.Count; i++)
            {
                int isFound = 0;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand("SELECT COUNT(character_id) " +
                                                        "FROM characters " +
                                                        "WHERE character_id = @character_id;", conn);
                        cmd.Parameters.AddWithValue("@character_id", characters[i].Id);
                        isFound = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (SqlException)
                {
                    throw;
                }
                if (isFound == 1)
                {
                    characters[i].InDatabase = true;
                }
            }
        }

        public List<Character> GetCharacterListForComicBook(int comicId)
        {
            List<Character> charsInComic = new List<Character>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT ch.character_id, ch.name " +
                                                    "FROM characters ch " +
                                                    "INNER JOIN comic_characters cc ON cc.character_id = ch.character_id " +
                                                    "INNER JOIN comics com ON com.comic_id = cc.comic_id " +
                                                    "WHERE com.comic_id = @comic_id;", conn);
                    cmd.Parameters.AddWithValue("@comic_id", comicId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        charsInComic.Add(GetCharacterFromReader(reader));
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return charsInComic;
        }

        private Character GetCharacterFromReader(SqlDataReader reader)
        {
            Character c = new Character
            {
                Id = Convert.ToInt32(reader["character_id"]),
                Name = Convert.ToString(reader["name"])
            };
            return c;
        }

        public List<CharacterCount> GetCollectionCharacterCount(int collectionId)
        {
            List<CharacterCount> charStats = new List<CharacterCount>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT cha.character_id, cha.name, COUNT(cha.character_id) AS count from comics com " +
                                   "JOIN collections_comics col on com.comic_id = col.comic_id " +
                                   "JOIN comic_characters com_cha on com.comic_id = com_cha.comic_id " +
                                   "JOIN characters cha ON com_cha.character_id = cha.character_id " +
                                   "WHERE col.collection_id = @collectionId " +
                                   "GROUP BY cha.character_id, cha.name;";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@collectionId", collectionId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        charStats.Add(new CharacterCount()
                        {
                            Character = GetCharacterFromReader(reader),
                            Count = Convert.ToInt32(reader["count"])
                        });
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return charStats;
        }
        public List<CharacterCount> GetTotalCollectionCharacterCount()
        {
            List<CharacterCount> charStats = new List<CharacterCount>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT cha.character_id, cha.name, COUNT(cha.character_id) AS count from comics com " +
                                   "JOIN collections_comics col on com.comic_id = col.comic_id " +
                                   "JOIN comic_characters com_cha on com.comic_id = com_cha.comic_id " +
                                   "JOIN characters cha ON com_cha.character_id = cha.character_id " +
                                   "GROUP BY cha.character_id, cha.name;";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        charStats.Add(new CharacterCount()
                        {
                            Character = GetCharacterFromReader(reader),
                            Count = Convert.ToInt32(reader["count"])
                        });
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return charStats;
        }
    }
}
