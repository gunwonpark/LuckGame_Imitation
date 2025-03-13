using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EnemyController : CreatureController
{
    private Transform[] _wayPoint;
    private Vector2 _nextPosition;
    private int _currentWayPointIndex;
    private int _maxWayPointIndex;

    #region Data
    [SerializeField] private float _moveSpeed = 1.0f;
    private float _slowDownDistance = 0.3f;
    private float _minMoveSpeed = 0.5f;
    private float _maxMoveSpeed = 1.0f;
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
            float distance = Vector2.Distance(transform.position, _nextPosition);

            // 부드러운 움직임 구현
            CaclateMoveSpeed(distance);

            if(distance < 0.02f)
            {
                SetNextPosition();
            }

            yield return null;
        }
    }


    private void CaclateMoveSpeed(float distance)
    {
        if(distance <= _slowDownDistance)
        {
            float t = distance / _slowDownDistance;
            t= t * t * (3f - 2f * t);
            _moveSpeed = Mathf.Lerp(_minMoveSpeed, _maxMoveSpeed, t);
        }
        else
        {
            _moveSpeed = 1.0f;
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
