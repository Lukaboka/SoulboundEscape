using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    [SerializeField] private int health = 100;
    [SerializeField] private Image healthBar;
 
    private int MAX_HEALTH = 100;
    
    public void SetHealth(int maxHealth, int health)
    {
        this.MAX_HEALTH = maxHealth;
        this.health = health;
        healthBar.fillAmount = 1;
    }

    public void Damage(int amount)
    {
        if(amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Damage");
        }

        this.health -= amount;
        this.healthBar.fillAmount = (float)health / (float)MAX_HEALTH;

        if(health <= 0)
        {
            healthBar.fillAmount = 0;
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative healing");
        }

        bool wouldBeOverMaxHealth = health + amount > MAX_HEALTH;

        if (wouldBeOverMaxHealth)
        {
            this.health = MAX_HEALTH;
        }
        else
        {
            this.health += amount;
        }
        healthBar.fillAmount = (float)health / (float)MAX_HEALTH;
    }

    private void Die()
    {
        Debug.Log("I am Dead!");
        Destroy(gameObject);
    }
}
