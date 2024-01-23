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
    [SerializeField] private GameObject getHitParticles;

    [Header("Survival Mode Components")]
    [SerializeField] private EnemiesSurvivalMode survivalMode;

    [SerializeField] private Image hpBar;

    [Header("Stats")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _damage;

    private Health _player;
    private bool _isDead = false;

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
        if(_currentHealth < 0 && !_isDead)
        {
            _isDead = true;
            hpBar.fillAmount = 0;
            animator.SetTrigger("Dead");
            enemyBehaviour.onDeathTrigger();
            DeletSelf();
        }
        else {
            getHitParticles.SetActive(false);
            getHitParticles.SetActive(true);
            enemyBehaviour.GotHit();
            animator.SetTrigger("GetHit");
        }
    }

    public void Attack()
    {
        _player = hitbox.GetIfPlayerIsInHitbox();
        if (_player != null)
        {
            _player.GetComponentInParent<PlayerCombat>().GetHit();
            AudioManager.instance.Hit();
            Debug.Log("Hit player!");
            _player.Damage(_damage);
        }
        _player = null;
    }

    private void DeletSelf()
    {
        if(survivalMode)
        {
            survivalMode.EnemyDied();
            gameObject.SetActive(false);
            return;
        }

        Destroy(gameObject, 2f);
    }
}
