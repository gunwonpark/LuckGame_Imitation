
// 게임에 사용될 전체적인 데이터 관리
public class GameData
{
    public float gold;
    public float coin;
    public float spawnLevel;
    public float spawnCount;
}

public class GameManager
{
    private GameData _gameData = new GameData();

    public GameData GameData { get { return _gameData; } }

    public float Gold
    {
        get { return _gameData.gold; }
        set { _gameData.gold = value; }
    }

    public float Coin
    {
        get { return _gameData.coin; }
        set { _gameData.coin = value; }
    }

    public float SpawnLevel
    {
        get { return _gameData.spawnLevel; }
        set { _gameData.spawnLevel = value; }
    }

    public float SpawnCount
    {
        get { return _gameData.spawnCount; }
        set { _gameData.spawnCount = value; }
    }

    public void Init()
    {
        _gameData.gold = 0;
        _gameData.coin = 0;
        _gameData.spawnLevel = 1;
        _gameData.spawnCount = 0;
    }

} 