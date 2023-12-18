using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private EnemyCombat enemyCombat;

    [Header("Stats")] 
    [SerializeField] private EnemyData data;
    [SerializeField] private Health _health;
    
    [Header("Movement")]
    [SerializeField]  private Transform target;
    [SerializeField] private float visionRange;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float rotateSpeed = 20f;

    [Header("Attack")]
    [SerializeField] private int damage;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 5f;
    private bool attacking = false;

    private bool isDead = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        SetEnemyValues();
    }

    void FixedUpdate()
    {
        if(isDead) { return; }
        if(attacking) { return; }

        float distance = Vector3.Distance(target.position, rb.position);

        if (visionRange < distance) {
            Stop();
            return;
        }
        if (distance > attackRange)
        {
            animator.SetBool("moving", true);
            Move();
        }

        if (distance <= attackRange)
        {
            Debug.Log("Attack!");
            attacking = true;
            animator.SetBool("attacking", true);
            animator.SetBool("moving", false);
            rb.velocity = Vector3.zero;
            StartCoroutine(AttackingRoutine());
        }
        
    }

    private void Stop()
    {
        animator.SetBool("moving", false);
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
    }

    private void Move()
    {
        Vector3 direction = target.position - rb.position;

        direction.Normalize();

        rb.angularVelocity = Vector3.Cross(direction, transform.forward) * -rotateSpeed;

        rb.velocity = transform.forward * speed;
    }

    private void SetEnemyValues()
    {
        enemyCombat.SetStats(data.hp, data.damage);
        damage = data.damage;
        speed = data.speed;
    }

    private IEnumerator AttackingRoutine()
    {
        yield return new WaitForSeconds(.25f);
        enemyCombat.Attack();
        yield return new WaitForSeconds(.75f);
        //TODO: Add attack
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(attackCooldown - 1f);
        attacking = false;
    }

    public void GotHit()
    {
        animator.SetTrigger("Hit");
    }

    public void onDeathTrigger()
    {
        isDead = true;
    }
}
