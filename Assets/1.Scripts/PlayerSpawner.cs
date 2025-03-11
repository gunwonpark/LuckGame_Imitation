using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// 단순한 빠른 구현
public class PlayerSpawner : MonoBehaviour
{
    #region Data
    public Tile[] playerSpawnPosition;
    public PlayerController playerPrefab;
    public PlayerController playerPrefab2;
    public GameObject SpawnEffect;
    public Transform spawnPos;
    #endregion

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        
    }

    public void Spawn()
    {
        // 임시 테스트를 위한 랜덤 오브젝트 찾기
        int value = Random.Range(0, 2);

        PlayerController player = value == 0 ? playerPrefab : playerPrefab2;

        int index = FindEmptyIndex(player);

        // 어처피 20마리로 제한되어 있어서 그럴일은 없다
        if (index == -1)
        {
            Debug.Log("더이상 생성할 수 없습니다.");
            return;
        }

        GameObject go = Instantiate(SpawnEffect, spawnPos.position, Quaternion.identity);

        // DOTween 시퀀스 생성
        Sequence sequence = DOTween.Sequence();

        Vector3[] path = GetPosition(index);

        go.transform.DOPath(path, 0.5f, PathType.Linear)
            .SetEase(Ease.Linear).OnComplete(() =>
            {
                Managers.Resource.Destroy(go);
                ShowPlayer(index);
                RePositionPlayer(index);
            });

        SpawnPlayer(player, index);
    }


    public Vector3[] GetPosition(int index, int cnt = 20)
    {
        Vector3[] positions = new Vector3[cnt];

        Vector3[] points = new Vector3[]
        {
            spawnPos.position,
            spawnPos.position + Vector3.up * 15f,
            playerSpawnPosition[index].transform.position + Vector3.up * 2f,
            playerSpawnPosition[index].transform.position
        };

        for (int i = 0; i < cnt; i++)
        {
            float t = i / (float)(cnt - 1);
            Vector3 pos = Bezier(points[0], points[1], points[2], points[3], t);
            positions[i] = pos;
        }

        return positions;
    }
    private Vector3 Bezier(Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3, float t)
    {
        Vector3 M0 = Vector3.Lerp(P0, P1, t);
        Vector3 M1 = Vector3.Lerp(P1, P2, t);
        Vector3 M2 = Vector3.Lerp(P2, P3, t);

        Vector3 B0 = Vector3.Lerp(M0, M1, t);
        Vector3 B1 = Vector3.Lerp(M1, M2, t);

        return Vector3.Lerp(B0, B1, t);
    }

    private void ShowPlayer(int index)
    {
        playerSpawnPosition[index].ShowPlayer();
    }

    private void SpawnPlayer(PlayerController player, int index)
    {
        playerSpawnPosition[index].SpawnPlayer(player);
    }

    //플레이어의 탄생 지점에 1개 이상이 있다면 위치를 조정해 준다
    private void RePositionPlayer(int index)
    {
        playerSpawnPosition[index].RePositionPlayer(index);
    }

    // 현재는 플레이어의 해쉬값을 통해 찾아내지만 추후에는 player의 고유 id값을 통해 찾아낸다
    private int FindPreviousIndex(PlayerController player)
    {
        int index = -1;
        int id = player.ID;

        for(int i = 0; i < playerSpawnPosition.Length; i++)
        {
            if(playerSpawnPosition[i].PlayerID == id &&
                playerSpawnPosition[i].CanSpawn)
            {
                index = i;
                break;
            }
        }

        return index;
    }

    private int FindEmptyIndex(PlayerController player)
    {
        int index = FindPreviousIndex(player);

        if (index != -1) return index;

        for (int i = 0; i < playerSpawnPosition.Length; i++)
        {
            if (playerSpawnPosition[i].PlayerCount == 0)
            {
                index = i;
                break;
            }
        }

        return index;
    }
}
