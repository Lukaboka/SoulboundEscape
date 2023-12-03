using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]  private Transform target;
    private Rigidbody rb;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float rotateSpeed = 20f;

    [Header("Attack")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 5f;
    private bool attacking = false;

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void FixedUpdate()
    {
        //TODO: Add conditions for the Movement
        float distance = Vector3.Distance(target.position, rb.position);
        if (distance > attackRange)
        {
            animator.SetBool("moving", true);
            Move();
        }

        if (!attacking && distance <= attackRange)
        {
            animator.SetBool("attacking", true);
            animator.SetBool("moving", false);
            attacking = true;
            StartCoroutine(AttackingRoutine());
        }
    }

    private void Move()
    {
        Vector3 direction = target.position - rb.position;

        direction.Normalize();

        rb.angularVelocity = Vector3.Cross(direction, transform.forward) * -rotateSpeed;

        rb.velocity = transform.forward * speed;
    }


    private IEnumerator AttackingRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        //TODO: Add attack
        animator.SetBool("attacking", false);
        attacking = false;
    }
}
