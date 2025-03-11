using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private bool _canSpawn = true;
    public bool CanSpawn => _players.Count < MaxPlayerCount;

    public int PlayerID { get; private set; } = -1;
    private List<PlayerController> _players = new List<PlayerController>();
    
    public int PlayerCount => _players.Count;
    public int MaxPlayerCount { get; private set; } = 3;

    public void SpawnPlayer(PlayerController player)
    {
        PlayerController newPlayer = Instantiate(player, this.transform.position, Quaternion.identity);

        PlayerID = newPlayer.ID;

        _players.Add(newPlayer);
        newPlayer.gameObject.SetActive(false);
    }
    public void DeSpawnPlayer()
    {
        Managers.Resource.Destroy(_players[_players.Count - 1].gameObject);
        _players.RemoveAt(_players.Count - 1);
    }

    public void ShowPlayer()
    {
        foreach (var player in _players)
        {
            player.gameObject.SetActive(true);
        }
    }

    public void RePositionPlayer(int index)
    {
        if (PlayerCount == 1) return;

        Vector3 startPosition = transform.position + Vector3.up * 0.3f;
        Vector3 endPosition = transform.position + Vector3.down * 0.3f;

        float spacing = 0.6f / (PlayerCount - 1);

        Vector3 curPosition = startPosition;
        int count = 0;
        foreach (var player in _players)
        {
            Vector3 get_left_right_position = count++ % 2 == 0 ? Vector3.left : Vector3.right;
            player.transform.position = curPosition + get_left_right_position * 0.3f;
            curPosition += Vector3.down * spacing;
        }
    }


}
