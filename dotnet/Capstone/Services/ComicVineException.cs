using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Services
{

    [Serializable]
    public class ComicVineException : Exception
    {
        public ComicVineException() { }
        public ComicVineException(string message) : base(message) { }
        public ComicVineException(string message, Exception inner) : base(message, inner) { }
        protected ComicVineException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
