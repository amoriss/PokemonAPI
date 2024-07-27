using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Pokemon_Battle_Sim_V2.Models
{
    public class Pokemon
    {
        private readonly HttpClient _client;
        private const string pokeURL = "https://pokeapi.co/api/v2/";

        // Constructors
        public Pokemon(HttpClient client)
        {
            _client = client;
        }

        public Pokemon() { }

        // Properties
        public string Name { get; set; }
        public string Nature { get; set; }
        public string FrontSprite { get; set; }
        public string BackSprite { get; set; }
        public int Level { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public List<string> TypeList { get; set; } = new List<string>();
        public List<double> StatList { get; set; } = new List<double>();
        public List<Move> MoveList { get; set; } = new List<Move>();

        // Methods
        public void IChooseYou(string name, int level, string nature)
        {
            var response = _client.GetStringAsync(pokeURL + $"pokemon/{name.ToLower()}").Result;
            var data = JObject.Parse(response);

            Name = name;
            Level = level;
            Nature = nature;
            Weight = (double)data.GetValue("weight") / 10;
            Height = (double)data.GetValue("height") / 10;

            var sprites = data.SelectToken("sprites");
            FrontSprite = sprites.SelectToken("front_default").ToString();
            BackSprite = sprites.SelectToken("back_default").ToString();

            var types = data.SelectToken("types");
            foreach (var type in types)
            {
                string typeName = type.SelectToken("type").SelectToken("name").ToString();
                TypeList.Add(typeName);
            }

            var stats = data.SelectToken("stats");
            foreach (var stat in stats)
            {
                int baseStat = (int)stat.SelectToken("base_stat");
                StatList.Add(baseStat);
            }
        }

        public void ChooseMoves(string m1, string m2, string m3, string m4)
        {
            MoveList.Add(GetMoveDetails(m1));
            MoveList.Add(GetMoveDetails(m2));
            MoveList.Add(GetMoveDetails(m3));
            MoveList.Add(GetMoveDetails(m4));
        }

        private Move GetMoveDetails(string moveName)
        {
            var moveResponse = JObject.Parse(_client.GetStringAsync(pokeURL + $"move/{moveName.ToLower()}").Result);
            var moveEffects = moveResponse.SelectToken("effect_entries");
            return new Move
            {
                MoveName = moveResponse.SelectToken("name").ToString(),
                MoveType = moveResponse.SelectToken("type.name").ToString(),
                MoveDamage = moveResponse.SelectToken("damage_class.name").ToString(),
                MoveDesc = moveEffects[0].SelectToken("short_effect").ToString(),
                MovePower = (int)moveResponse.SelectToken("power"),
                MoveAccuracy = (int)moveResponse.SelectToken("accuracy"),
                MoveUses = (int)moveResponse.SelectToken("pp"),
                MovePriority = (int)moveResponse.SelectToken("priority")
            };
        }

        public void StatCalculation(int[] iv, int[] ev)
        {
            var natureResponse = _client.GetStringAsync(pokeURL + $"nature/{Nature.ToLower()}").Result;
            var natureData = JObject.Parse(natureResponse);

            StatList[0] = (((2 * StatList[0] + iv[0] + (ev[0] / 4)) * Level) / 100) + Level + 10;

            for (int i = 1; i < StatList.Count; i++)
            {
                double stat = ((((2 * StatList[i] + iv[i] + (ev[i] / 4)) * Level) / 100) + 5);
                StatList[i] = stat;
            }

            ApplyNatureEffects(natureData, "increased_stat.name", 1.1);
            ApplyNatureEffects(natureData, "decreased_stat.name", 0.9);
        }

        private void ApplyNatureEffects(JObject natureData, string effectType, double multiplier)
        {
            switch ((string)natureData.SelectToken(effectType))
            {
                case "attack":
                    StatList[1] *= multiplier;
                    break;
                case "defense":
                    StatList[2] *= multiplier;
                    break;
                case "special-attack":
                    StatList[3] *= multiplier;
                    break;
                case "special-defense":
                    StatList[4] *= multiplier;
                    break;
                case "speed":
                    StatList[5] *= multiplier;
                    break;
            }
        }
    }
}
