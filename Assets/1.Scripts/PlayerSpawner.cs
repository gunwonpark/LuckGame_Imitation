using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// �ܼ��� ���� ����
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
        for(int i = 0; i < playerSpawnPosition.Length; i++)
        {
            playerSpawnPosition[i].Init(i);
        }
    }

    public void Spawn()
    {
        // �ӽ� �׽�Ʈ�� ���� ���� ������Ʈ ã��
        int value = Random.Range(0, 2);

        PlayerController player = value == 0 ? playerPrefab : playerPrefab2;

        int index = FindEmptyIndex(player);

        // ��ó�� 20������ ���ѵǾ� �־ �׷����� ����
        if (index == -1)
        {
            Debug.Log("���̻� ������ �� �����ϴ�.");
            return;
        }

        GameObject go = Instantiate(SpawnEffect, spawnPos.position, Quaternion.identity);

        // DOTween ������ ����
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

    //�÷��̾��� ź�� ������ 1�� �̻��� �ִٸ� ��ġ�� ������ �ش�
    private void RePositionPlayer(int index)
    {
        playerSpawnPosition[index].RePositionPlayer(index);
    }

    // ����� �÷��̾��� �ؽ����� ���� ã�Ƴ����� ���Ŀ��� player�� ���� id���� ���� ã�Ƴ���
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

    private Tile focusTile = null;
    private Tile targetTile = null;
    public SelectPointer pointer;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.TryGetComponent(out Tile tile))
                {
                    focusTile = tile;
                    pointer.SetCurEdge(focusTile.transform.position);
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if(focusTile != null)
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.TryGetComponent(out Tile tile))
                    {
                        targetTile = tile;
                    }
                }
            }

            if(focusTile != null && targetTile != null)
            {
                pointer.SetTargetEdge(targetTile.transform.position);
                pointer.SetLine(focusTile.transform.position, targetTile.transform.position);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (focusTile != null && targetTile == null)
            {
                // Ÿ�Ͽ� �ִ� �÷��̾� ������ ��� �ش�
            }

            if (focusTile != null && targetTile != null)
            {
                SwapPosition(focusTile, targetTile);
            }

            pointer.ResetAll();
            focusTile = null;
        }
    }

    private void SwapPosition(Tile focusTile, Tile targetTile)
    {
        int focusIndex = focusTile.PositionIndex;
        int targetIndex = targetTile.PositionIndex;
        
        Vector3 focusPos = focusTile.transform.position;
        Vector3 targetPos = targetTile.transform.position;

        focusTile.transform.position = targetPos;
        targetTile.transform.position = focusPos;

        playerSpawnPosition[focusIndex] = targetTile;
        playerSpawnPosition[targetIndex] = focusTile;

        RePositionPlayer(focusIndex);
        RePositionPlayer(targetIndex);
    }
}
