using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _attackRange = 1.5f;
    private float _attackDamage = 10f;
    private float _attackSpeed = 1.0f;

    private Vector3 _attackPosition;

    public int ID;
    private void OnEnable()
    {
    }

    public void Init(Vector3 attackPosition)
    {
        _attackPosition = attackPosition;
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        while (true)
        {

        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
