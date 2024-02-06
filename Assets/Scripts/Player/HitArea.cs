using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitArea : MonoBehaviour
{
    private int _damage = 0;

    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.enabled = false;
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<EnemyCombat>(out EnemyCombat enemy))
            {
                enemy.GetHit(_damage);
            }
        }
    }

    public void AttackStateChanged(bool state)
    {
        _collider.enabled = state;
    }
}
