using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private EnemyData data;
    [SerializeField] private int damage;

    [Header("Attack")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 5f;
    private bool attacking = false;

    // Active world is true if OverWorld
    // Active world is false if UnderWorld
    private bool activeWorld = true;

    [SerializeField] private Sword overworldSword;
    [SerializeField] private Sword underworldSword;

    public void ChangeActiveWorld()
    {
        activeWorld = !activeWorld;
    }

    public void Attack(bool state)
    {
        if(activeWorld) { overworldSword.AttackStateChanged(state); }
        else            { underworldSword.AttackStateChanged(state); }
    }

    public void GetHit(int dmg)
    {

    }
}
