using System.Collections.Generic;
using UnityEngine;

public interface IDictionaryLoader<TKey, TValue>
{
    Dictionary<TKey, TValue> MakeDictionary();
}

//
public class DataManager
{
    public Dictionary<int, PlayerData> PlayerDatas { get; private set; } = new Dictionary<int, PlayerData>(); 
    public void Init()
    {
        PlayerDatas = LoadJson<PlayerDataLoader, int, PlayerData>("PlayerData").MakeDictionary();
    }

    private Loader LoadJson<Loader, TKey, TValue>(string path) where Loader : IDictionaryLoader<TKey, TValue>, new()
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
