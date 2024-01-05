using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController: MonoBehaviour
{

    [Header("Player Cameras")] 
    [SerializeField] private CameraAnimationHandler cameraAnimationHandler;
    
    [Header("Player Stats")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotateSpeed = 720;
    
    [Header("Animators")]
    [SerializeField] private Animator animatorCharacterOverworld;
    [SerializeField] private Animator animatorCharacterUnderworld;

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

    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Dash()
    {
        _rigidbody.velocity = Vector3.zero;
        movementSpeed += 2;
        if (_swapped && !animatorCharacterUnderworld.GetBool(Attacking))
        {
            animatorCharacterUnderworld.SetTrigger(Dashing);
        }
        else if (!_swapped && !animatorCharacterOverworld.GetBool(Attacking))
        {
            animatorCharacterOverworld.SetTrigger(Dashing);
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
        // Mirrors the rotation along the z,y-plane rotated 45° along the y-axis
        var lookingDirection = transform.forward;
        var mirroredLookingDirection = Vector3.Reflect(-lookingDirection, new Vector3(-0.71f, 0, 0.71f));
        transform.rotation = Quaternion.LookRotation(mirroredLookingDirection);

        _swapped = !_swapped;
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
        if (_swapped && !animatorCharacterUnderworld.GetBool(Attacking))
        {
            animatorCharacterUnderworld.SetBool(Attacking, true);
        }
        else if (!_swapped && !animatorCharacterOverworld.GetBool(Attacking))
        {
            animatorCharacterOverworld.SetBool(Attacking, true);
        }
        StartCoroutine(FinishAttack(1f));
    }

    private IEnumerator FinishAttack(float attackInterval)
    {
        yield return new WaitForSeconds(attackInterval);
        player.Attack(false);
        animatorCharacterUnderworld.SetBool(Attacking, false);
        animatorCharacterOverworld.SetBool(Attacking, false);
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
            return;
        }
        
        animatorCharacterOverworld.SetBool(Running, true);
        animatorCharacterUnderworld.SetBool(Running, true);
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

    public void onDeathTrigger()
    {
        _dead = true;
    }
}