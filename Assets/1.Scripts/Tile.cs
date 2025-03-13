using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private List<PlayerController> _players = new List<PlayerController>();
    public int PlayerID { get; private set; } = -1;
    public int PlayerCount => _players.Count;
    public int MaxPlayerCount { get; private set; } = 3;
    public int PositionIndex { get; private set; } = -1;
    public bool CanSpawn => _players.Count < MaxPlayerCount;

    public void Init(int index)
    {
        PositionIndex = index;
    }

    public void SpawnPlayer(int id)
    {
        PlayerController newPlayer = Managers.Resource.Instantiate(Managers.Data.PlayerDatas[id].prefabName, this.transform.position).GetComponent<PlayerController>();

        PlayerID = id;

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
        PositionIndex = index;

        if (PlayerCount == 0) return;

        if (PlayerCount == 1)
        {
            _players[0].transform.DOMove(transform.position, 1f);
            return;
        }

        Vector3 startPosition = transform.position + Vector3.up * 0.3f;
        Vector3 endPosition = transform.position + Vector3.down * 0.3f;

        float spacing = 0.6f / (PlayerCount - 1);

        Vector3 curPosition = startPosition;
        int count = 0;
        foreach (var player in _players)
        {
            Vector3 get_left_right_position = count++ % 2 == 0 ? Vector3.left : Vector3.right;
            player.transform.DOMove(curPosition + get_left_right_position * 0.3f, 1f);
            curPosition += Vector3.down * spacing;
        }
    }

    

}
