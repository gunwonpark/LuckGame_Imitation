using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DB", menuName = "Scriptable Objects/DB")]
public class DB : ScriptableObject
{
    public List<PlayerData> playerDatas = new List<PlayerData>();

}
