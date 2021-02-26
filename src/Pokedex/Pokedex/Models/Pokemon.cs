using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pokedex.Models
{
    public class Pokemon
    {

        public int Height { get; set; }
        public int Weight { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }
        

        public PokemonSprites Sprites { get; set; }
        public PokemonType[] Types { get; set; }
    }

    public class PokemonSprites
    {
        [JsonProperty("front_default")]
        public string FrontDefault { get; set; }

        [JsonProperty("front_shiny")]
        public string FrontShiny { get; set; }

        [JsonProperty("front_female")]
        public string FrontFemale { get; set; }

        [JsonProperty("front_shiny_female")]
        public string FrontShinyFemale { get; set; }

        [JsonProperty("back_default")]
        public string BackDefault { get; set; }

        [JsonProperty("back_shiny")]
        public string BackShiny { get; set; }

        [JsonProperty("back_female")]
        public string BackFemale { get; set; }

        [JsonProperty("back_shiny_female")]
        public string BackShinyFemale { get; set; }

    }


    public class PokemonType
    {
        public int Slot { get; set; }

        public NamedAPIResource Type { get; set; }
    }

}
