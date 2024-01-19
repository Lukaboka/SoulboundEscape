using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private EnemyCombat enemyCombat;

    [Header("Stats")]
    [SerializeField] private bool isBoss = false;
    [SerializeField] private int bossMoveCount = 0;
    [SerializeField] private EnemyData data;

    [Header("Movement")]
    [SerializeField]  private Transform target;
    [SerializeField] private float visionRange;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float rotateSpeed = 20f;
    [SerializeField] private NavMeshAgent agent;

    [Header("Attack")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 5f;
    
    private bool _attacking;
    private bool _isDead;
    private Transform _transform;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        SetEnemyValues();
        agent.destination = target.position;
    }

    void FixedUpdate()
    {
        if(_attacking) { return; }

        float distance = Vector3.Distance(target.position, rb.position);

        if (visionRange < distance) {
            Stop();
            return;
        }
        if (distance > attackRange)
        {
            agent.destination = target.position;
            animator.SetBool("moving", true);
            Move();
        } else {
            animator.SetBool("moving", false);
        }


        if (distance <= attackRange)
        {
            _attacking = true;
            if (isBoss) { BossAttack(); }
            else
            {
                animator.SetBool("attacking", true);
                animator.SetBool("moving", false);
                rb.velocity = Vector3.zero;
                StartCoroutine(AttackingRoutine());
            }
        }
        
    }

    private void Stop()
    {
        animator.SetBool("moving", false);
        agent.destination = target.position;
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
        speed = data.speed;
    }

    private IEnumerator AttackingRoutine()
    {
        yield return new WaitForSeconds(.25f);
        enemyCombat.Attack();
        yield return new WaitForSeconds(.75f);
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(attackCooldown - 1f);
        _attacking = false;
    }

    public void GotHit()
    {
        animator.SetTrigger("GetHit");
    }

    public void onDeathTrigger()
    {
        _isDead = true;
    }

    private void BossAttack()
    {
        rb.velocity = Vector3.zero;

        var attackType = "Attack" + Random.Range(1, bossMoveCount).ToString();

        animator.SetTrigger(attackType);
        animator.SetBool("moving", false);
        StartCoroutine(BossAttackingRoutine());
    }
    
    private IEnumerator BossAttackingRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        _attacking = false;
    }
}
