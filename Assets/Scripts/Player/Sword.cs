using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    private bool _isAttacking = false;
    private int _damage = 0;

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isAttacking && other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<EnemyCombat>(out EnemyCombat enemy))
            {
                enemy.GetHit(_damage);
            }
        }
    }

    public void AttackStateChanged(bool state)
    {
        _isAttacking = state;
    }
}
