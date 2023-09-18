using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private PlayerState currentState;

    [SerializeField] public IdlePlayerState idle = new IdlePlayerState();
    [SerializeField] public WalkPlayerState walking = new WalkPlayerState();
    [SerializeField] public RunningPlayerState running = new RunningPlayerState();
    [SerializeField] public JumpPlayerState jump = new JumpPlayerState();
    
    [SerializeField, Space(10)]
    private Rigidbody2D _rb;

    public float gravity = 40f;

    [HideInInspector]
    public bool isGrounded = true;
    private float _timeSinceGrounded = 0;
    [SerializeField]
    private float _coyoteTime = 0.1f;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundMask;

    [SerializeField] public PlayerInputHandler input;


    public Vector2 velocity = Vector2.zero;

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
        
        idle.OnValidate(this);
        walking.OnValidate(this);
        running.OnValidate(this);
        jump.OnValidate(this);
    }

    private void Awake()
    {
        idle.Awake();
        walking.Awake();
        running.Awake();
        jump.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        idle.Start();
        walking.Start();
        running.Start();
        jump.Start();
        
        currentState = idle;
        currentState.Enter();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState != null)
            currentState.UpdateState();
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
        Debug.Log("Transitioned to " + currentState + " state");
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
