using System;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace Pokemon_Battle_Sim_V2.Models
{
	public class PokeAPI : IPokeAPI
	{
        private readonly HttpClient _client;

        public PokeAPI(HttpClient client)
        {
            _client = client;
        }
        public async Task<PokeBasic> GetBasicInfo(string name, int level, string nature)
		{			
            string pokeURL = $"https://pokeapi.co/api/v2/pokemon/{name}";
			//string natureURL = $"https://pokeapi.co/api/v2/nature/";

			var response = await _client.GetStringAsync(pokeURL);
            var data = JObject.Parse(response);

			return new PokeBasic
			{
				Name = data["name"].ToString(),
				Level = level,
				Nature = nature
			};
        }
		
	}
}

