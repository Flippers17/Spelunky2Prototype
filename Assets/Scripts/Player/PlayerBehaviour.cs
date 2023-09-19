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
    public TakeDamagePlayerState takeDamage = new TakeDamagePlayerState();
    
    [SerializeField, Space(10)]
    private Rigidbody2D _rb;
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
    [SerializeField] private Transform _groundCheck;
    public LayerMask _groundMask;

    [SerializeField, Space(10)] public PlayerInputHandler input;

    [HideInInspector]
    public Vector2 velocity = Vector2.zero;
    [HideInInspector]
    public int facingDirection = 1;

    private void OnValidate()
    {
        if(_rb == null)
            if(!TryGetComponent(out _rb))
                Debug.LogWarning("PlayerBehaviour is missing RigidBody2D reference", this);
        
        if(!_groundCheck)
            Debug.LogWarning("No GroundCheck Transform has been assigned!", this);
        
        if(!input)
            if(!TryGetComponent(out input))
                Debug.LogWarning("PlayerBehaviour is missing PlayerInputHandler reference", this);
        
        if(!_health)
            if(!TryGetComponent(out _health))
                Debug.LogWarning("PlayerBehaviour is missing Health reference", this);
        
        idle.OnValidate(this);
        walking.OnValidate(this);
        running.OnValidate(this);
        crouching.OnValidate(this);
        jump.OnValidate(this);
        ledgeGrab.OnValidate(this);
        takeDamage.OnValidate(this);
    }

    private void Awake()
    {
        idle.Awake();
        walking.Awake();
        running.Awake();
        crouching.Awake();
        jump.Awake();
        ledgeGrab.Awake();
        takeDamage.Awake();
    }

    void Start()
    {
        idle.Start();
        walking.Start();
        running.Start();
        crouching.Start();
        jump.Start();
        ledgeGrab.Start();
        takeDamage.Start();
        
        currentState = idle;
        currentState.Enter();
    }

    private void OnEnable()
    {
        _health.OnDie += Die;
    }

    private void OnDisable()
    {
        _health.OnDie -= Die;
    }

    void Update()
    {
        if(currentState != null)
            currentState.UpdateState();

        if (currentState != takeDamage && timeSinceDamaged < _invincibillityTime)
            timeSinceDamaged += Time.deltaTime;
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
        if (currentState == takeDamage || timeSinceDamaged < _invincibillityTime)
            return;

        TransitionToState(takeDamage);
        velocity = knockback;
        Debug.Log("Took damage");
    }

    public void Die()
    {
        //Transition to Dead state
        Debug.Log("Player Died");
    }


    public float GetGravity()
    {
        if (velocity.y > .4f && input.HoldingJump())
            return gravity * .75f;

        return gravity;
    }

    private bool IsGrounded()
    {
        bool grounded = Physics2D.OverlapBox((Vector2)_groundCheck.position, (Vector2)_groundCheck.localScale, 0, _groundMask);

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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_groundCheck.position, _groundCheck.localScale);
    }
}
