using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    private Dictionary<int, PlayerData> playerDatas = new Dictionary<int, PlayerData>();

    public void Init()
    {
        DB db = Managers.Resource.Load<DB>("DB");
        foreach (var playerData in db.playerDatas)
        {
            playerDatas.Add(playerData.id, playerData);
        }
    }
}
