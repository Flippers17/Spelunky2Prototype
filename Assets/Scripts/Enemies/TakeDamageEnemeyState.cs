using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageEnemeyState : EnemyState
{
    [SerializeField] private float _stunTime = 0.3f;
    private float timer = 0;

    public override void Awake(SnakeBehaviour enemy)
    {
        base.Awake(enemy);
    }
    public override void Start() { }

    public override void Enter()
    {
        _enemy.velocity.x = 0;
        timer = 0;
        _anim.SetBool("Taking Damage", true);
    }

    public override void UpdateState()
    {
        if (_enemy.IsGrounded())
            _enemy.velocity.y = -1;
        else
            _enemy.velocity.y -= _enemy.gravity * Time.deltaTime;

        if (timer < _stunTime)
            timer += Time.deltaTime;
        else
            TransitionFromState();
    }

    public override void FixedUpdateState()
    {
        
    }

    protected virtual void TransitionFromState()
    {
        _enemy.TransitionToState(_enemy.walking);
    }

    public override void Exit()
    {
        _anim.SetBool("Taking Damage", false);
    }
}
