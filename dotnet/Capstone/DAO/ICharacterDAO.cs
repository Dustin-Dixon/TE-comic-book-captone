using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface ICharacterDAO
    {
        public Character GetCharacterById(int charId);
        public bool AddCharacterToTable(Character character);
        public bool LinkCharacterToComic(int charId, int comicId);
        public void CheckDatabaseForCharacters(List<Character> characters);
        public List<Character> GetCharacterListForComicBook(int comicId);
    }
}