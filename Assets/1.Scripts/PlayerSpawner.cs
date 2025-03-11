using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// �ܼ��� ���� ����
public class PlayerSpawner : MonoBehaviour
{
    #region Data
    public Transform[] playerSpawnPosition;
    public GameObject playerPrefab;
    public GameObject playerPrefab2;
    public GameObject SpawnEffect;
    public Transform spawnPos;
    #endregion

    // Ư�� ������ ����� ������ ����Ǿ� �ִ���
    private List<int> _currentPlayerVolume = new List<int>();
    // Ư�� ID�� ������ ��� ������ ����Ǿ��ִ���
    private Dictionary<int, HashSet<int>> _playersInfo = new Dictionary<int, HashSet<int>>();
    // Ư�� ������ ����� ���ֵ�
    private List<HashSet<GameObject>> _players = new List<HashSet<GameObject>>();
    private void Start()
    {
        Init();
    }
    public void Init()
    {
        _currentPlayerVolume.Clear();
        for (int i = 0; i < playerSpawnPosition.Length; i++)
        {
            _currentPlayerVolume.Add(0);
            _players.Add(new HashSet<GameObject>());
        }
    }

    public void Spawn()
    {
        // �ӽ� �׽�Ʈ�� ���� ���� ������Ʈ ã��
        int value = Random.Range(0, 2);

        GameObject player = value == 0 ? playerPrefab : playerPrefab2;

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
            playerSpawnPosition[index].position + Vector3.up * 2f,
            playerSpawnPosition[index].position
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
        foreach (var player in _players[index])
        {
            if(player != null && player.activeInHierarchy == false)
            {
                player.SetActive(true);
            }
        }
    }

    private void SpawnPlayer(GameObject player, int index)
    {
        GameObject newPlayer = Instantiate(player, playerSpawnPosition[index].position, Quaternion.identity);

        _currentPlayerVolume[index]++;
        if (!_playersInfo.ContainsKey(player.name.GetHashCode()))
        {
            _playersInfo.Add(player.name.GetHashCode(), new HashSet<int>());
        }

        _playersInfo[player.name.GetHashCode()].Add(index);
        _players[index].Add(newPlayer);

        newPlayer.SetActive(false);
    }

    //�÷��̾��� ź�� ������ 1�� �̻��� �ִٸ� ��ġ�� ������ �ش�
    private void RePositionPlayer(int index)
    {
        if (_currentPlayerVolume[index] == 1) return;

        Vector3 startPosition = playerSpawnPosition[index].position + Vector3.up * 0.3f;
        Vector3 endPosition = playerSpawnPosition[index].position + Vector3.down * 0.3f;

        float spacing = 0.6f / (_currentPlayerVolume[index] - 1);

        Vector3 curPosition = startPosition;
        int count = 0;
        foreach (var player in _players[index])
        {
            Vector3 get_left_right_position = count++ % 2 == 0 ? Vector3.left : Vector3.right;
            player.transform.position = curPosition + get_left_right_position * 0.3f;
            curPosition += Vector3.down * spacing;

        }
    }

    // ����� �÷��̾��� �ؽ����� ���� ã�Ƴ����� ���Ŀ��� player�� ���� id���� ���� ã�Ƴ���
    private int FindPreviousIndex(GameObject player)
    {
        int hashCode = player.name.GetHashCode();
        int index = -1;

        if (_playersInfo.TryGetValue(hashCode, out HashSet<int> value))
        {
            Debug.Log(value);
            foreach (var item in value)
            {
                // �� ������ �÷��̾��� ���� max������ ���� ���� �Ѵ�
                if (_currentPlayerVolume[item] < 3)
                {
                    index = item;
                    break;
                }
            }
        }

        return index;
    }

    private int FindEmptyIndex(GameObject player)
    {
        int index = FindPreviousIndex(player);

        if (index != -1) return index;

        for (int i = 0; i < _currentPlayerVolume.Count; i++)
        {
            if (_currentPlayerVolume[i] == 0)
            {
                index = i;
                break;
            }
        }

        return index;
    }
}
