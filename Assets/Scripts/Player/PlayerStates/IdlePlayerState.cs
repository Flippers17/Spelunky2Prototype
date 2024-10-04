using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IdlePlayerState : PlayerState
{
    
    //public override void Awake(){}
    //public override void Start(){}

    public override void Enter()
    {
        _input.OnAttack += TransitionToAttack;
        _player.velocity.x = 0;
    }

    public override void UpdateState()
    {
        _anim.SetBool("Jumping", !_player.isGrounded);
        
        if(_input.GetHorizontalMoveInput() != 0)
            _stateMachine.TransitionToState(_stateMachine.walking);

        if (_input.RememberJumpInput())
            TransitionToJump();
        else if(_input.HoldingCrouch() && _player.isGrounded)
            _stateMachine.TransitionToState(_stateMachine.crouching);
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
                        _stateMachine.TransitionToState(_stateMachine.jump);
                    }
            }
        }
        else
            _player.velocity.y = -1;

        if (_input.HoldingClimb() && LadderCheck(out Transform ladder))
        {
            _player.transform.position = ladder.position;
            _stateMachine.TransitionToState(_stateMachine.climbLadder);
        }
    }


    private bool LadderCheck(out Transform ladder)
    {
        ladder = null;

        Collider2D[] results = Physics2D.OverlapCircleAll(_player.transform.position, .2f, 1 << 12);
        if (results != null && results.Length != 0)
        {
            ladder = results[0].transform;
            return true;
        }

        return false;
    }
    
    public override void Exit()
    {
        _input.OnAttack -= TransitionToAttack;
    }

    private void TransitionToJump()
    {
        if (!_player.WithinCoyoteTime())
            return;

        _stateMachine.TransitionToState(_stateMachine.jump);
    }

    private void TransitionToAttack()
    {
        _stateMachine.TransitionToState(_stateMachine.attack);
    }
    
}
