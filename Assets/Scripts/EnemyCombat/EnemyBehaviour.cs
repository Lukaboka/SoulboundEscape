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
    [SerializeField] private int damage;
<<<<<<< HEAD:Assets/Scripts/EnemyBehaviour.cs

    [Header("Movement")]
    [SerializeField] private GameObject player;
    private Rigidbody rb;
=======
    private bool isDead = false;
    
    [Header("Movement")]
    [SerializeField]  private Transform target;
    [SerializeField] private float visionRange;
>>>>>>> 73418797fc31ca0043d09d645f6abe14ec7cbd5d:Assets/Scripts/EnemyCombat/EnemyBehaviour.cs
    [SerializeField] private float speed = 2f;
    [SerializeField] private float rotateSpeed = 20f;

    [Header("Attack")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 5f;
    private bool attacking = false;

<<<<<<< HEAD:Assets/Scripts/EnemyBehaviour.cs
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
=======
    void Start()
    {
        SetEnemyValues();
>>>>>>> 73418797fc31ca0043d09d645f6abe14ec7cbd5d:Assets/Scripts/EnemyCombat/EnemyBehaviour.cs
    }

    void FixedUpdate()
    {
        if(isDead) { return; }
        if(attacking) { return; }

        //TODO: Add conditions for the Movement
<<<<<<< HEAD:Assets/Scripts/EnemyBehaviour.cs
        float distance = Vector3.Distance(player.transform.position, rb.position);
=======
        float distance = Vector3.Distance(target.position, rb.position);

        if (visionRange < distance) {
            Stop();
            return;
        }

>>>>>>> 73418797fc31ca0043d09d645f6abe14ec7cbd5d:Assets/Scripts/EnemyCombat/EnemyBehaviour.cs
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
        Vector3 direction = player.transform.position - rb.position;

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

    }

    public void onDeathTrigger()
    {
        isDead = true;
    }
}
