using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform[] _wayPoint;
    private Vector2 _nextPosition;
    private int _currentWayPointIndex;
    private int _maxWayPointIndex;

    #region Data
    [SerializeField] private float _moveSpeed = 1.0f;
    #endregion
    public void Init(Transform[] wayPoint)
    {
        _wayPoint = wayPoint;
        _maxWayPointIndex = _wayPoint.Length;
        _currentWayPointIndex = 0;

        // 초기 위치 설정
        this.transform.position = _wayPoint[_currentWayPointIndex].position;
        SetNextPosition();

        StartCoroutine(CheckWayPointMove());
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _nextPosition, _moveSpeed * Time.deltaTime);
    }


    private IEnumerator CheckWayPointMove()
    {
        while (true)
        {
            if(Vector2.Distance(transform.position, _nextPosition) < 0.02f)
            {
                SetNextPosition();
            }

            yield return null;
        }
    }

    private void SetNextPosition()
    {
        _currentWayPointIndex++;

        if (_currentWayPointIndex >= _maxWayPointIndex)
        {
            _currentWayPointIndex = 0;
        }

        _nextPosition = _wayPoint[_currentWayPointIndex].position;
    }

}
