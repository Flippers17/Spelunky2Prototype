using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IdleEnemyState : EnemyState
{
    [SerializeField] private float _idleTime = 2f;

    private float timer = 0;

    public override void Enter()
    {
        timer = 0;
    }

    public override void UpdateState()
    {
        _enemy.velocity.x = 0;

        if (_enemy.IsGrounded())
            _enemy.velocity.y = -1;
        else
            _enemy.velocity.y -= _enemy.gravity * Time.deltaTime;

        if (timer < _idleTime)
            timer += Time.deltaTime;
        else
            _enemy.TransitionToState(_enemy.walking);
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void Exit()
    {
        
    }
}
