using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IdlePlayerState : PlayerState
{
    
    public override void Awake(){}
    public override void Start(){}

    public override void Enter()
    {
        _input.OnAttack += TransitionToAttack;
        _player.velocity.x = 0;
    }

    public override void UpdateState()
    {
        _anim.SetBool("Jumping", !_player.isGrounded);
        
        if(_input.GetHorizontalMoveInput() != 0)
            _player.TransitionToState(_player.walking);

        if (_input.RememberJumpInput())
            TransitionToJump();
        else if(_input.HoldingCrouch() && _player.isGrounded)
            _player.TransitionToState(_player.crouching);
    }

    public override void FixedUpdateState()
    {
        if (!_player.isGrounded)
        {
            _player.velocity.y -= _player.GetGravity() * Time.fixedDeltaTime;
            
            if(_player.velocity.y < 0)
            {
                Collider2D enemy = Physics2D.OverlapBox(_player.groundCheck.position, _player.groundCheck.localScale, 0, 1 << 9);
                if (enemy != null)
                    if (enemy.TryGetComponent(out EnemyBehaviour eBehaviour))
                    {
                        eBehaviour.TakeDamage(1, Vector2.zero);
                        _player.TransitionToState(_player.jump);
                    }
            }
        }
        else
            _player.velocity.y = -1;
    }

    public override void Exit()
    {
        _input.OnAttack -= TransitionToAttack;
    }

    private void TransitionToJump()
    {
        if (!_player.WithinCoyoteTime())
            return;

        _player.TransitionToState(_player.jump);
    }

    private void TransitionToAttack()
    {
        if(!_player.isGrounded)
            return;
        
        _player.TransitionToState(_player.attack);
    }
    
    public override void OnValidate(PlayerBehaviour player)
    {
        base.OnValidate(player);
    }
}
