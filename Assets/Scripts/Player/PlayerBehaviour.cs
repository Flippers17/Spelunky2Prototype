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
    [SerializeField] public JumpPlayerState jump = new JumpPlayerState();
    
    [SerializeField, Space(10)]
    private Rigidbody2D _rb;

    public float gravity = 40f;

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
        jump.OnValidate(this);
    }

    private void Awake()
    {
        idle.Awake();
        walking.Awake();
        jump.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        idle.Start();
        walking.Start();
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
    

    public bool IsGrounded()
    {
        return Physics2D.OverlapBox((Vector2)_groundCheck.position, (Vector2)_groundCheck.localScale, 0, _groundMask);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_groundCheck.position, _groundCheck.localScale);
    }
}
