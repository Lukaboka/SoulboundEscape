using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using System;


public class PlayerController: MonoBehaviour
{

    [Header("Player Cameras")] 
    [SerializeField] private CameraAnimationHandler cameraAnimationHandler;
    
    [Header("Player Stats")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotateSpeed = 720;
    
    [Header("Animators")]
    [SerializeField] private Animator animatorCharacterOverworld;
    [SerializeField] private Animator animatorDummyUnderworld;
    [SerializeField] private Animator animatorCharacterUnderworld;
    [SerializeField] private Animator animatorDummyOverworld;

    [Header("Attack Stats")]
    [SerializeField] private PlayerCombat player;
    [SerializeField] private float attackSpeed = 1.5f;
    [SerializeField] private float attackDelay = 0.3f;
    [SerializeField] private float attackDamage = 1;
    
    private bool _swapped;
    private Vector2 _inputVector;
    private Vector3 _direction;
    private Rigidbody _rigidbody;
    private Transform _transform;
    private static readonly int Running = Animator.StringToHash("Running");
    private static readonly int Attacking = Animator.StringToHash("Attacking");
    private static readonly int Dashing = Animator.StringToHash("Dashing");
    private static readonly int Dead = Animator.StringToHash("Dead");

    private bool _dead = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (_dead) { return; }
        
        GetDirection();
        Look();
        
        // World flip mechanic
        if (Input.GetKeyDown(KeyCode.F) == true)
        {
            if (!cameraAnimationHandler.IsInAnimation())
            {
                player.ChangeActiveWorld();
                cameraAnimationHandler.SwapWorld();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dash();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }

    }

    private void FixedUpdate()
    {
        if (_dead) { return; }
        Move();
    }

    private void Dash()
    {
        _rigidbody.velocity = Vector3.zero;
        movementSpeed += 2;
        if (_swapped && !animatorCharacterUnderworld.GetBool(Attacking))
        {
            animatorCharacterUnderworld.SetTrigger(Dashing);
            animatorDummyOverworld.SetTrigger(Dashing);
        }
        else if (!_swapped && !animatorCharacterOverworld.GetBool(Attacking))
        {
            animatorCharacterOverworld.SetTrigger(Dashing);
            animatorDummyUnderworld.SetTrigger(Dashing);
        }
        StartCoroutine(FinishDash(1f));
    }

    private IEnumerator FinishDash(float interval)
    {
        yield return new WaitForSeconds(interval);
        movementSpeed -= 2;
    }

    public void AdjustControls()
    {
        
        _swapped = !_swapped;
        
    }

    // Fetches user keyboard input and puts it into _input vector
    private void GetDirection() {
        
        _direction = new Vector3(_inputVector.x, 0, _inputVector.y).normalized;
        
    }

    public void Attack()
    {
        player.Attack(true);
        if (_swapped && !animatorCharacterUnderworld.GetBool(Attacking))
        {
            animatorCharacterUnderworld.SetBool(Attacking, true);
            animatorDummyOverworld.SetBool(Attacking, true);
        }
        else if (!_swapped && !animatorCharacterOverworld.GetBool(Attacking))
        {
            animatorCharacterOverworld.SetBool(Attacking, true);
            animatorDummyUnderworld.SetBool(Attacking, true);
        }
        StartCoroutine(FinishAttack(1f));
    }

    private IEnumerator FinishAttack(float attackInterval)
    {
        yield return new WaitForSeconds(attackInterval);
        player.Attack(false);
        animatorCharacterUnderworld.SetBool(Attacking, false);
        animatorCharacterOverworld.SetBool(Attacking, false);
        animatorDummyUnderworld.SetBool(Attacking, false);
        animatorDummyOverworld.SetBool(Attacking, false);
    }

    public void GetInput(InputAction.CallbackContext context)
    {
        _inputVector = context.ReadValue<Vector2>();
    }

    // Rotates the player character's rotation towards the current input vector 
    private void Look()
    {
        if (_direction == Vector3.zero)
        {
            animatorCharacterOverworld.SetBool(Running, false);
            animatorCharacterUnderworld.SetBool(Running, false);
            animatorDummyOverworld.SetBool(Running, false);
            animatorDummyUnderworld.SetBool(Running, false);
            return;
        }
        
        animatorCharacterOverworld.SetBool(Running, true);
        animatorCharacterUnderworld.SetBool(Running, true);
        animatorDummyOverworld.SetBool(Running, true);
        animatorDummyUnderworld.SetBool(Running, true);
        var position = transform.position;
        var relative = (position + _direction.ToIso()) - position;
        var rot = Quaternion.LookRotation(relative, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotateSpeed * Time.deltaTime);
    }

    // Moves along the current forward vector of the player character
    private void Move()
    {
        if (_direction == Vector3.zero)
        {
            Vector3 position = transform.position;
            
            _rigidbody.velocity = Vector3.zero;
            _transform.position = new Vector3(position.x, 0, position.z);
            return;
        }
        _rigidbody.MovePosition(_transform.position + _transform.forward * (_direction.normalized.magnitude * movementSpeed * Time.deltaTime));
    }

    public void SetMovementSpeed(float speed)
    {
        movementSpeed = speed;
    }

    private void Interact()
    {
        animatorCharacterOverworld.SetTrigger("Interact");
        animatorCharacterUnderworld.SetTrigger("Interact");
    }

    public void GetHit(bool activeWorld)
    {
        if (activeWorld) { animatorCharacterOverworld.SetTrigger("GetHit"); }
        else             { animatorCharacterUnderworld.SetTrigger("GetHit"); }
    }

    public void onDeathTrigger()
    {
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.velocity = Vector3.zero;

        animatorCharacterOverworld.SetTrigger("Dead");
        animatorCharacterUnderworld.SetTrigger("Dead");
        _dead = true;
        GetComponent<PlayerCombat>().Dead();

        FindObjectOfType<GameManager_SurvivalMode>().LoseLevel();
    }
    
}