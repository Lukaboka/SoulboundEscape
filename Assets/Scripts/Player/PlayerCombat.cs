using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerController _controller;

    [Header("Stats")]
    [SerializeField] private EnemyData data;
    [SerializeField] private int damage;
    [SerializeField] private Health _overworldHealth;
    [SerializeField] private Health _underworlddHealth;


    [Header("Attack")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 5f;
    private bool attacking = false;

    // Active world is true if OverWorld
    // Active world is false if UnderWorld
    private bool activeWorld = true;

    [SerializeField] private Sword overworldSword;
    [SerializeField] private Sword underworldSword;

    [SerializeField] private ParticleSystem swordParticle_overworld;
    [SerializeField] private ParticleSystem swordParticle_underworld;

    [SerializeField] private ParticleSystem gethitParticle_overworld;
    [SerializeField] private ParticleSystem gethitParticle_underworld;

    [SerializeField] private ParticleSystem deadParticle_overworld;
    [SerializeField] private ParticleSystem deadParticle_underworld;

    public bool isDead = false;

    private void Awake()
    {
        SetData();
    }

    public void ChangeActiveWorld()
    {
        if (isDead) { return; }
        activeWorld = !activeWorld;
    }

    public void Attack(bool state)
    {
        if (isDead) { return; }
        if (activeWorld) {
            overworldSword.AttackStateChanged(state);
            swordParticle_overworld.gameObject.SetActive(true);
            swordParticle_overworld.Play();
            StartCoroutine(stopParticle(swordParticle_overworld.totalTime));
        } else {
            underworldSword.AttackStateChanged(state);
            swordParticle_underworld.gameObject.SetActive(true);
            swordParticle_underworld.Play();
            StartCoroutine(stopParticle(swordParticle_underworld.totalTime));
        }
    }

    IEnumerator stopParticle(float time)
    {
        yield return new WaitForSeconds(0.1f);
        swordParticle_overworld.gameObject.SetActive(false);
        swordParticle_underworld.gameObject.SetActive(false);
    }

    public void GetHit()
    {
        if(isDead) { return; }
        if (activeWorld) { gethitParticle_overworld.Play(); _controller.GetHit(true); }
        else             { gethitParticle_underworld.Play(); _controller.GetHit(false); }
    }

    public void Dead()
    {
        isDead = true;
        deadParticle_overworld.gameObject.SetActive(true);
        deadParticle_underworld.gameObject.SetActive(true);
    }


    public int GetDamage()
    {
        return data.damage;
    }

    public int GetHealth()
    {
        return data.hp;
    }

    public float GetSpeed()
    {
        return data.speed;
    }

    public void SetData()
    {
        _controller.SetMovementSpeed(data.speed);
        overworldSword.SetDamage(data.damage);
        underworldSword.SetDamage(data.damage);
        _underworlddHealth.SetHealth(data.hp, data.hp);
        _overworldHealth.SetHealth(data.hp, data.hp);

        _speed = data.speed;
        _damage = data.damage;
        _hp = data.hp;
    }

    [SerializeField] private float _speed = 0;
    [SerializeField] private int _damage = 0;
    [SerializeField] private int _hp = 0;

    public void Powerup(PowerupType type, PowerupStrength strength)
    {
        switch (type) {
            case PowerupType.Damage:
                switch (strength)
                {
                    case PowerupStrength.low:
                        _damage += 10;
                        break;
                    case PowerupStrength.mid:
                        _damage += 25;
                        break;
                    case PowerupStrength.high:
                        _damage += 50;
                        break;
                }
                AudioManager.instance.Heal2();
                overworldSword.SetDamage(_damage);
                underworldSword.SetDamage(_damage);
                break;
            case PowerupType.Health:
                switch (strength)
                {
                    case PowerupStrength.low:
                        _hp = 10;
                        break;
                    case PowerupStrength.mid:
                        _hp = 25;
                        break;
                    case PowerupStrength.high:
                        _hp = 50;
                        break;
                }
                if (activeWorld) { _overworldHealth.Heal(_hp); }
                else             { _underworlddHealth.Heal(_hp); }
                break;
            case PowerupType.Speed:
                switch (strength)
                {
                    case PowerupStrength.low:
                        _speed += .2f;
                        break;
                    case PowerupStrength.mid:
                        _speed += .5f;
                        break;
                    case PowerupStrength.high:
                        _speed += 1f;
                        break;
                }
                AudioManager.instance.Heal2();
                _controller.SetMovementSpeed(_speed);
                break;
        }
    }
}
