using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, IDamageable
{
    [SerializeField] private Health _health;
    [SerializeField] private float _invincibillityTime = 0.2f;
    private float _timeSinceHit = 0;

    public Animator anim;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _gravity = 30;
    [SerializeField] private Transform _groundCheck;
    public LayerMask groundMask;

    protected EnemyState currentState;

    [HideInInspector] public int facingDirection = 1;
    private SpriteRenderer _sprite;

    public float gravity
    {
        get
        {
            return _gravity;
        }
        private set
        {
            _gravity = value;
        }
    }

    [HideInInspector] public Vector2 velocity = Vector2.zero;

    protected virtual void OnValidate()
    {

        if(_health == null)
            if(!TryGetComponent(out _health))
                Debug.LogWarning("EnemyBehaviour is missing Health reference", this);
        
        if(anim == null)
            if(!TryGetComponent(out anim))
                Debug.LogWarning("EnemyBehaviour is missing Animator reference", this);
        
        if(_rb == null)
            if(!TryGetComponent(out _rb))
                Debug.LogWarning("EnemyBehaviour is missing Rigidbody2D reference", this);
    }

    protected virtual void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    protected virtual void OnEnable()
    {
        _health.OnDie += Die;
    }

    protected virtual void OnDisable()
    {
        _health.OnDie -= Die;
    }

    protected virtual void Update()
    {
        if (facingDirection == -1)
        {
            if(_sprite)
                _sprite.flipX = true;
        }
        else
        {
            if(_sprite)
                _sprite.flipX = false;
        }
        
        if (_timeSinceHit < _invincibillityTime)
            _timeSinceHit += Time.deltaTime;
        
        if(currentState != null)
            currentState.UpdateState();
    }

    protected virtual void FixedUpdate()
    {
        if(currentState != null)
            currentState.FixedUpdateState();
        
        _rb.velocity = velocity;
    }

    public virtual void TakeDamage(int damage, Vector2 knockback)
    {
        if(!CanBeHit())
            return;
        
        _timeSinceHit = 0;
        _health.TakeDamage(damage);
    }

    public void Die()
    {
        Debug.Log(this.gameObject.name + " has died");
        Destroy(gameObject);
    }

    public void TransitionToState(EnemyState targetState)
    {
        if(targetState == null)
            return;
        
        currentState.Exit();
        currentState = targetState;
        currentState.Enter();
    }

    public bool IsGrounded()
    {
        bool grounded = Physics2D.OverlapBox((Vector2)_groundCheck.position, (Vector2)_groundCheck.localScale, 0, groundMask);

        return grounded;
    }
    
    public bool CanBeHit()
    {
        return _timeSinceHit > _invincibillityTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_groundCheck.position, _groundCheck.localScale);
    }
}
