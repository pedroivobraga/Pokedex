using System.Collections.Generic;

namespace Pokedex.Models
{
    public class Paged<T>
    {
        public int Count { get; set; }
        
        public string Next { get; set; }

        public string Previous { get; set; }

        public List<T> Results { get; set; }

        public Paged()
        {
            Results = new List<T>();
        }

    }
}
