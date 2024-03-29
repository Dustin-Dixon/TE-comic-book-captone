﻿using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public interface ITagDAO
    {
        public Tag AddTagToDatabase(string description);
        public bool LinkTagToComic(int comicId, int tagId);
        public Tag GetTagByDescription(string description);
        public bool IsTagLinkedToComic(int comicId, int tagId);
        public List<Tag> GetTagListForComicBook(int comicId);
        public List<Tag> GetAllTags();
        public int GetCountOfTagAcrossDatabase(int tagId);
    }
}