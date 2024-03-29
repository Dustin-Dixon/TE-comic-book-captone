﻿using Capstone.Models;
using Capstone.Models.Stats;
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
        public List<CharacterCount> GetCollectionCharacterCount(int collectionId);
        public List<CharacterCount> GetTotalCollectionCharacterCount();
    }
}