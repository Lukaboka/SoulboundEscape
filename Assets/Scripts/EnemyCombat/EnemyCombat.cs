using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCombat : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyBehaviour enemyBehaviour;
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyHitArea hitbox;

    [SerializeField] private Image hpBar;

    [Header("Stats")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _damage;

    private Health _player;

    public void SetStats(int maxHealth, int damage)
    {
        _maxHealth = maxHealth;
        _currentHealth = maxHealth;
        _damage = damage;
        hpBar.fillAmount = 1;
    }

    public void GetHit(int damage)
    {
        _currentHealth -= damage;
        hpBar.fillAmount = (float)_currentHealth / (float)_maxHealth;
        if(_currentHealth < 0)
        {
            hpBar.fillAmount = 0;
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
            _player.Damage(_damage);
        }
        _player = null;
    }

    private void DeletSelf()
    {
        Destroy(gameObject, 2f);
    }
}
