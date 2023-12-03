
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private float attackSpeed = 1.5f;
    [SerializeField] private float attackDelay = 0.3f;
    [SerializeField] private float attackDamage = 1;
    
    private bool _swapped;
    private Vector2 _inputVector;
    private Vector3 _direction;
    private Rigidbody _rigidbody;
    private static readonly int Running = Animator.StringToHash("Running");
    private static readonly int Attacking = Animator.StringToHash("Attacking");

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        cameraOverworld.enabled = true;
        cameraUnderworld.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        GetDirection(_swapped);
        Look();
        
        // World flip mechanic
        if (Input.GetKeyDown(KeyCode.F) == true)
        {
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

    }

    private void FixedUpdate()
    {
        Move();
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
        if (_swapped && !_animatorCharacterUnderworld.GetBool(Attacking))
        {
            _animatorCharacterUnderworld.SetBool(Attacking, true);
        }
        else if (!_swapped && !_animatorCharacterOverworld.GetBool(Attacking))
        {
            _animatorCharacterOverworld.SetBool(Attacking, true);
        }
    }

    private void FinishAttack()
    {
        
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
}