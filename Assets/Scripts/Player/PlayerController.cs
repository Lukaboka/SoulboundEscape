using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

public class PlayerController: MonoBehaviour
{
    
    [Header("Player Cameras")]
    [SerializeField] private Camera cameraOverworld;
    [SerializeField] private Camera cameraUnderworld;
    
    [Header("Player Stats")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotateSpeed = 720;
    
    [Header("Animators")]
    [SerializeField] private Animator _animatorCharacterOverworld;
    [SerializeField] private Animator _animatorCharacterUnderworld;

    [Header("Attack Stats")]
    [SerializeField] private PlayerCombat player;
    [SerializeField] private float attackSpeed = 1.5f;
    [SerializeField] private float attackDelay = 0.3f;
    [SerializeField] private float attackDamage = 1;
    
    private bool _swapped;
    private Vector2 _inputVector;
    private Vector3 _direction;
    private Rigidbody _rigidbody;
    private static readonly int Running = Animator.StringToHash("Running");
    private static readonly int Attacking = Animator.StringToHash("Attacking");
    private static readonly int Dashing = Animator.StringToHash("Dashing");
    private static readonly int Dead = Animator.StringToHash("Dead");

    private bool _dead = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        cameraOverworld.enabled = true;
        cameraUnderworld.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_dead) { return; }
        
        GetDirection(_swapped);
        Look();
        
        // World flip mechanic
        if (Input.GetKeyDown(KeyCode.F) == true)
        {
            player.ChangeActiveWorld();
            if (cameraOverworld.enabled)
            {
                cameraOverworld.enabled = false;
                cameraUnderworld.enabled = true;
            }
            else
            {
                cameraOverworld.enabled = true;
                cameraUnderworld.enabled = false;
            }
                
            // Mirrors the rotation along the z,y-plane rotated 45° along the y-axis
            var lookingDirection = transform.forward;
            var mirroredLookingDirection = Vector3.Reflect(-lookingDirection, new Vector3(-0.71f, 0, 0.71f));
            transform.rotation = Quaternion.LookRotation(mirroredLookingDirection);

            _swapped = !_swapped;
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
        if (_swapped && !_animatorCharacterUnderworld.GetBool(Attacking))
        {
            _animatorCharacterUnderworld.SetTrigger(Dashing);
        }
        else if (!_swapped && !_animatorCharacterOverworld.GetBool(Attacking))
        {
            _animatorCharacterOverworld.SetTrigger(Dashing);
        }
        StartCoroutine(FinishDash(1f));
    }

    private IEnumerator FinishDash(float interval)
    {
        yield return new WaitForSeconds(interval);
        movementSpeed -= 2;
    }

    // Fetches user keyboard input and puts it into _input vector
    private void GetDirection(bool swapped) {
        
        if (!swapped)
        {
            _direction = new Vector3(_inputVector.x, 0, _inputVector.y).normalized;
        }
        else
        {
            _direction = new Vector3(_inputVector.x, 0, -_inputVector.y).normalized;
        }
    }

    public void Attack()
    {
        player.Attack(true);
        if (_swapped && !_animatorCharacterUnderworld.GetBool(Attacking))
        {
            _animatorCharacterUnderworld.SetBool(Attacking, true);
        }
        else if (!_swapped && !_animatorCharacterOverworld.GetBool(Attacking))
        {
            _animatorCharacterOverworld.SetBool(Attacking, true);
        }
        StartCoroutine(FinishAttack(1f));
    }

    private IEnumerator FinishAttack(float attackInterval)
    {
        yield return new WaitForSeconds(attackInterval);
        player.Attack(false);
        _animatorCharacterUnderworld.SetBool(Attacking, false);
        _animatorCharacterOverworld.SetBool(Attacking, false);
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
            _animatorCharacterOverworld.SetBool(Running, false);
            _animatorCharacterUnderworld.SetBool(Running, false);
            return;
        }
        
        _animatorCharacterOverworld.SetBool(Running, true);
        _animatorCharacterUnderworld.SetBool(Running, true);
        var position = transform.position;
        var relative = (position + _direction.ToIso()) - position;
        var rot = Quaternion.LookRotation(relative, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotateSpeed * Time.deltaTime);
    }

    // Moves along the current forward vector of the player character
    private void Move()
    {
        var transform1 = transform;
        _rigidbody.MovePosition(transform1.position + transform1.forward * (_direction.normalized.magnitude * movementSpeed * Time.deltaTime));
    }

    public void SetMovementSpeed(float speed)
    {
        movementSpeed = speed;
    }

    private void Interact()
    {
        _animatorCharacterOverworld.SetTrigger("Interact");
        _animatorCharacterUnderworld.SetTrigger("Interact");
    }

    public void GetHit(bool activeWorld)
    {
        if (activeWorld) { _animatorCharacterOverworld.SetTrigger("GetHit"); }
        else             { _animatorCharacterUnderworld.SetTrigger("GetHit"); }
    }

    public void onDeathTrigger()
    {
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.velocity = Vector3.zero;

        _animatorCharacterOverworld.SetTrigger("Dead");
        _animatorCharacterUnderworld.SetTrigger("Dead");
        _dead = true;
        GetComponent<PlayerCombat>().Dead();

        FindObjectOfType<GameManager_SurvivalMode>().LoseLevel();
    }
}