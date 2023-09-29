using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[RequireComponent(typeof(Health))]
public interface IDamageable
{
    void TakeDamage(int damage, Vector2 knockback);

    void Die();

    bool CanBeHit();
}


public class PlayerBehaviour : MonoBehaviour , IDamageable
{
    private PlayerState currentState;

    public IdlePlayerState idle = new IdlePlayerState();
    public WalkPlayerState walking = new WalkPlayerState();
    public RunningPlayerState running = new RunningPlayerState();
    public CrouchPlayerState crouching = new CrouchPlayerState();
    public JumpPlayerState jump = new JumpPlayerState();
    public LedgeGrabPlayerState ledgeGrab = new LedgeGrabPlayerState();
    public ClimbDownLedgePlayerState climbDown = new ClimbDownLedgePlayerState();
    public ClimbUpLedgePlayerState climbUp = new ClimbUpLedgePlayerState();
    public TakeDamagePlayerState takeDamage = new TakeDamagePlayerState();
    public AttackPlayerState attack = new AttackPlayerState();
    public EnterDoorPlayerState enterDoor = new EnterDoorPlayerState();
    public DeadPlayerState dead = new DeadPlayerState();
    public ClimbLadderPlayerState climbLadder = new ClimbLadderPlayerState();
    
    [SerializeField, Space(10)]
    private Rigidbody2D _rb;
    public Animator anim;
    private SpriteRenderer _sprite;
    [SerializeField, Space(10)]
    private Health _health;
    [SerializeField]
    private float _invincibillityTime = 1f;
    [HideInInspector]
    public float timeSinceDamaged = 5;

    public float gravity = 40f;

    [HideInInspector]
    public bool isGrounded = true;
    private float _timeSinceGrounded = 0;
    [SerializeField, Space(10)]
    private float _coyoteTime = 0.1f;
    public Transform groundCheck;
    public LayerMask groundMask;

    [SerializeField, Space(10)] public PlayerInputHandler input;
    [SerializeField] private HurtBox _whipHurtBox;
    
    public Transform _itemPickupPoint;
    [HideInInspector]
    public Item currentHeldItem = null;

    public EnterDoorEventPort EnterDoorEvent;

    [HideInInspector]
    public Vector2 velocity = Vector2.zero;
    [HideInInspector]
    public int facingDirection = 1;
    
    private void OnValidate()
    {
        _sprite = GetComponent<SpriteRenderer>();
        
        if(_rb == null)
            if(!TryGetComponent(out _rb))
                Debug.LogWarning("PlayerBehaviour is missing RigidBody2D reference", this);
        
        if(!groundCheck)
            Debug.LogWarning("No GroundCheck Transform has been assigned!", this);
        
        if(!input)
            if(!TryGetComponent(out input))
                Debug.LogWarning("PlayerBehaviour is missing PlayerInputHandler reference", this);
        
        if(!_health)
            if(!TryGetComponent(out _health))
                Debug.LogWarning("PlayerBehaviour is missing Health reference", this);
        
        if(!anim)
            if(!TryGetComponent(out anim))
                Debug.LogWarning("PlayerBehaviour is missing Animator reference", this);
        
        idle.OnValidate(this);
        walking.OnValidate(this);
        running.OnValidate(this);
        crouching.OnValidate(this);
        jump.OnValidate(this);
        ledgeGrab.OnValidate(this);
        climbDown.OnValidate(this);
        climbUp.OnValidate(this);
        takeDamage.OnValidate(this);
        attack.OnValidate(this);
        enterDoor.OnValidate(this);
        dead.OnValidate(this);
        climbLadder.OnValidate(this);
    }

    private void Awake()
    {
        idle.Awake();
        walking.Awake();
        running.Awake();
        crouching.Awake();
        jump.Awake();
        ledgeGrab.Awake();
        climbDown.Awake();
        climbUp.Awake();
        takeDamage.Awake();
        attack.Awake();
        enterDoor.Awake();
        dead.Awake();
        climbLadder.Awake();
    }

    void Start()
    {
        idle.Start();
        walking.Start();
        running.Start();
        crouching.Start();
        jump.Start();
        ledgeGrab.Start();
        climbDown.Start();
        climbUp.Start();
        takeDamage.Start();
        attack.Start();
        enterDoor.Start();
        dead.Start();
        climbLadder.Start();
        
        currentState = idle;
        currentState.Enter();
    }

    private void OnEnable()
    {
        _health.OnDie += Die;
        EnterDoorEvent.OnEnterDoor += OnEnterDoor;
    }

    private void OnDisable()
    {
        _health.OnDie -= Die;
        EnterDoorEvent.OnEnterDoor -= OnEnterDoor;
    }

    void Update()
    {
        if (facingDirection == -1)
        {
            _sprite.flipX = true;
            _whipHurtBox.transform.localPosition = new Vector3(-1, _whipHurtBox.transform.localPosition.y, 0);
        }
        else
        {
            _sprite.flipX = false;
            _whipHurtBox.transform.localPosition = new Vector3(1, _whipHurtBox.transform.localPosition.y, 0);
        }
        
        if(currentState != null)
            currentState.UpdateState();

        if (currentState != takeDamage && timeSinceDamaged < _invincibillityTime)
        {
            anim.SetBool("Flickering", true);
            timeSinceDamaged += Time.deltaTime;
        }
        else
            anim.SetBool("Flickering", false);
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();

        if(currentState != null)
            currentState.FixedUpdateState();
        
        _rb.velocity = velocity;
        
    }
    

    public void TransitionToState(PlayerState targetState)
    {
        currentState.Exit();
        currentState = targetState;
        currentState.Enter();
        //Debug.Log("Transitioned to " + currentState + " state");
    }

    public void TakeDamage(int damage, Vector2 knockback)
    {
        //Transition to Take damage state
        if (currentState == takeDamage || timeSinceDamaged < _invincibillityTime || currentState == dead)
            return;

        _health.TakeDamage(damage);

        if (currentState != dead)
            TransitionToState(takeDamage);
        velocity = knockback;
        Debug.Log("Took damage");
    }

    public void Die()
    {
        //Transition to Dead state
        TransitionToState(dead);
        Debug.Log("Player Died");
    }


    public void ActivateWhipHurtbox()
    {
        if(_whipHurtBox == null)
            return;
        
        _whipHurtBox.enabled = true;
    }
    
    public void DeactivateWhipHurtbox()
    {
        if(_whipHurtBox == null)
            return;
        
        _whipHurtBox.enabled = false;
    }
    
    public float GetGravity()
    {
        if (velocity.y > .4f && input.HoldingJump())
            return gravity * .75f;

        return gravity;
    }

    private bool IsGrounded()
    {
        bool grounded = Physics2D.OverlapBox((Vector2)groundCheck.position, (Vector2)groundCheck.localScale, 0, groundMask);

        if (grounded)
            _timeSinceGrounded = 0;
        else
            _timeSinceGrounded += Time.fixedDeltaTime;

        return grounded;
    }

    public bool WithinCoyoteTime()
    {
        return _timeSinceGrounded < _coyoteTime;
    }

    public bool CanBeHit()
    {
        return !(timeSinceDamaged < _invincibillityTime);
    }

    private void OnEnterDoor()
    {
        if(currentState != enterDoor)
            TransitionToState(enterDoor);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, groundCheck.localScale);
    }
}
