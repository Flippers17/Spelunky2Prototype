using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WalkingEnemyState : EnemyState
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _walkTime = 3f;

    private float timer = 0;

    public override void Awake(SnakeBehaviour enemy)
    {
        base.Awake(enemy);
    }
    public override void Start() 
    {
        timer = Random.Range(0, _walkTime);
    }

    public override void Enter()
    {
        timer = 0;
        _anim.SetBool("Walking", true);
    }

    public override void UpdateState()
    {
        _enemy.velocity.x = _speed * _enemy.facingDirection;

        if (_enemy.IsGrounded())
            _enemy.velocity.y = -1;
        else
            _enemy.velocity.y -= _enemy.gravity * Time.deltaTime;

        if (timer < _walkTime)
            timer += Time.deltaTime;
        else
            _enemy.TransitionToState(_enemy.idle);
    }

    public override void FixedUpdateState()
    {
        if (Physics2D.Raycast(_enemy.transform.position, Vector2.right * _enemy.facingDirection, 0.6f, _enemy.groundMask) ||
            (!Physics2D.Raycast((Vector2)_enemy.transform.position + Vector2.right * (_enemy.facingDirection * 0.5f),
                Vector2.down, 0.6f, _enemy.groundMask) && _enemy.IsGrounded()))
        {
            _enemy.facingDirection *= -1;
        }    
    }

    public override void Exit()
    {
        _anim.SetBool("Walking", false);
    }
}
