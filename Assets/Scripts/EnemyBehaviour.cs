using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Stats")] 
    [SerializeField] private EnemyData data;
    [SerializeField] private int damage;

    [Header("Movement")]
    [SerializeField] private GameObject player;
    private Rigidbody rb;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float rotateSpeed = 20f;

    [Header("Attack")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 5f;
    private bool attacking = false;

    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        SetEnemyValues();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void FixedUpdate()
    {
        //TODO: Add conditions for the Movement
        float distance = Vector3.Distance(player.transform.position, rb.position);
        if (distance > attackRange)
        {
            animator.SetBool("moving", true);
            Move();
        }

        if (!attacking && distance <= attackRange)
        {
            animator.SetBool("attacking", true);
            animator.SetBool("moving", false);
            rb.velocity = Vector3.zero;
            attacking = true;
            StartCoroutine(AttackingRoutine());
        }
    }

    private void Move()
    {
        Vector3 direction = player.transform.position - rb.position;

        direction.Normalize();

        rb.angularVelocity = Vector3.Cross(direction, transform.forward) * -rotateSpeed;

        rb.velocity = transform.forward * speed;
    }

    private void SetEnemyValues()
    {
        GetComponent<Health>().SetHealth(data.hp, data.hp);
        damage = data.damage;
        speed = data.speed;
    }


    private IEnumerator AttackingRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        //TODO: Add attack
        animator.SetBool("attacking", false);
        attacking = false;
    }
}
