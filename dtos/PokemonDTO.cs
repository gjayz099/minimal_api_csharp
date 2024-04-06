using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using csharp.models;

namespace csharp.dtos
{
    public class PokemonDTO
    {
        public string name { get; set; } = String.Empty;
        public string type { get; set; } = String.Empty;
        public string img { get; set; } = String.Empty;

        public PokemonDTO() {}
        public PokemonDTO(Pokemon pokemon) =>
        (name, type, img) = (pokemon.name, pokemon.type, pokemon.img);
    }
}