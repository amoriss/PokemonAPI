namespace Pokemon_Battle_Sim_V2.Models;

public interface IPokeAPI
{
    Task<PokeBasic> GetBasicInfo(string name, int level, string nature);
}

