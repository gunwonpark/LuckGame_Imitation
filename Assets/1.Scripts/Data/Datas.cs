
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
    public PlayerRare rare;
    public int id;
    public string name;
}