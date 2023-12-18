using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private bool _isAttacking = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_isAttacking && other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<EnemyCombat>(out EnemyCombat enemy))
            {
                enemy.GetHit(10);
            }
            Debug.Log("Hit!");
        }
    }

    public void AttackStateChanged(bool state)
    {
        _isAttacking = state;
    }
}
