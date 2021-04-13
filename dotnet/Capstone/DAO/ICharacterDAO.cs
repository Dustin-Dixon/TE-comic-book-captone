using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface ICharacterDAO
    {
        public Character GetCharacterById(int charId);
        public void CheckDatabaseForCharacters(List<Character> characters);
    }
}