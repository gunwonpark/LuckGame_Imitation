using System;
using System.Collections;
using UnityEngine;

public enum PlayerState
{
    IDLE,
    ATTACK,
    SKILL,
}

public class PlayerController : CreatureController
{
    private float _attackRange = 1.5f;
    private float _attackDamage = 10f;
    private float _attackSpeed = 1.0f;
    private Transform _attackPosition;


    private EnemyController _target;
    private Coroutine _attackCoroutine;
    // ���� �Ÿ�Ȯ�ο� ����Ʈ
    [SerializeField] private GameObject _attackRangeEffect;
    [SerializeField] private GameObject _playerInfoUI;

    public PlayerState State { get; private set; } = PlayerState.IDLE;

    public void Init(Transform attackPosition)
    {
        _attackPosition = attackPosition;
    }

    private void Update()
    {
        
    }

    private void Attack()
    {
        _attackCoroutine = StartCoroutine(CoAttack());
    }

    private IEnumerator CoAttack()
    {
        while (_target != null)
        {
            Debug.Log($"Damage: {_target.name}");
            yield return new WaitForSeconds(_attackSpeed);
        }
    }

    private void CheckAttackState()
    {
        if (State != PlayerState.IDLE) return;

        Collider2D target = Physics2D.OverlapCircle(_attackPosition.position, _attackRange, 1 << LayerMask.NameToLayer("Enemey"));

        if(target != null && target.TryGetComponent(out EnemyController enemy))
        {
            _target = enemy;
            State = PlayerState.ATTACK;
        }
    }

    private void OnGUI()
    {
        if (_attackPosition == null) return;
        Gizmos.DrawWireSphere(_attackPosition.position, _attackRange);
    }

    //�÷��̾ Ŭ���ϸ� �÷��̾� ������ �����ش�
    //�� �ܿ� Ŭ�����̰ų� �巡�� ���̸� �÷��̾� ������ �����



    public void ShowPlayerInfo()
    {
        ShowAttackRange();
    }

    public void ShowAttackRange()
    {
        _attackRangeEffect.transform.localScale = new Vector3(_attackRange * 2, _attackRange * 2, 1);
        _attackRangeEffect.SetActive(true);
    }

    public void HideAttackRange()
    {
        _attackRangeEffect.SetActive(false);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

}
