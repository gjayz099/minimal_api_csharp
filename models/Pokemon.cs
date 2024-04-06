using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csharp.models
{
    public class Pokemon
    {
        public int id { get; set; }
        public string name { get; set; } = String.Empty;
        public string type { get; set; } = String.Empty;
        public string img { get; set; } = String.Empty;
    }
}