using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyBehaviour enemyBehaviour;
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyHitArea hitbox;

    [Header("Stats")]
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _damage;

    private PlayerCombat _player;

    public void SetStats(int maxHealth, int damage)
    {
        _currentHealth = maxHealth;
        _damage = damage;
    }

    public void GetHit(int damage)
    {
        _currentHealth -= damage;
        if(_currentHealth < 0)
        {
            animator.SetBool("Dead", true);
            enemyBehaviour.onDeathTrigger();
            DeletSelf();
        }
        else {
            enemyBehaviour.GotHit();
            animator.SetTrigger("GetHit");
        }
    }

    public void Attack()
    {
        // TODO: Check area for player, attack if player is inside
        _player = hitbox.GetIfPlayerIsInHitbox();
        if (_player != null)
        {
            Debug.Log("Hit player!");
            _player.GetHit(_damage);
        }
        _player = null;
    }

    private void DeletSelf()
    {
        Destroy(gameObject, 2f);
    }
}
