using DG.Tweening;
using UnityEngine;

public class SelectPointer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _line;
    [SerializeField] private GameObject _curEdge;
    [SerializeField] private GameObject _targetEdge;


    public void SetCurEdge(Vector3 position)
    {
        _curEdge.SetActive(true);
        _curEdge.transform.position = position;
        _curEdge.transform.DOScale(Vector3.one * 1.1f, 1f).SetLoops(-1, LoopType.Yoyo);
    }

    public void ResetAll()
    {
        _curEdge.SetActive(false);
        _targetEdge.SetActive(false);
        _line.gameObject.SetActive(false);

        _curEdge.transform.DOKill();
        _curEdge.transform.localScale = Vector3.one;
    }

    public void SetTargetEdge(Vector3 position)
    {
        _targetEdge.SetActive(true);
        _targetEdge.transform.position = position;
    }

    public void SetLine(Vector2 startPosition, Vector2 endPosition)
    {
        _line.gameObject.SetActive(true);

        _line.transform.position = startPosition;

        float distance = Vector2.Distance(startPosition, endPosition);
        _line.size = new Vector2(_line.size.x, distance);

        Vector2 direction = endPosition - startPosition;
        _line.transform.up = direction;
    }
}
