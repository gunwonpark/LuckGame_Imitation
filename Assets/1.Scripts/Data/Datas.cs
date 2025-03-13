using System.Collections;
using System.Collections.Generic;

public enum PlayerRare
{
    Common,
    Rare,
    Epic,
    Legendary,

    Max
}


[System.Serializable]
public class PlayerData 
{
    public int id;
    public string prefabName;
    public PlayerRare rare;
}

[System.Serializable]
public class PlayerDataLoader : IDictionaryLoader<int, PlayerData>
{
    public List<PlayerData> playerDatas = new List<PlayerData>();
    public Dictionary<int, PlayerData> MakeDictionary()
    {
        Dictionary<int, PlayerData> dic = new Dictionary<int, PlayerData>();

        foreach (var player in playerDatas)
        {
            dic.Add(player.id, player);
        }

        return dic;
    }
}
