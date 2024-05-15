using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClimbLadderPlayerState : PlayerState
{
    [SerializeField]
    private float _climbSpeed = 2f;

    private bool _onLadder = true;
    
    public override void Awake(){}
    public override void Start(){}

    public override void Enter()
    {
        _input.OnAttack += TransitionToAttack;
        _player.velocity.x = 0;
        _onLadder = true;
        _anim.SetBool("Climbing Ladder", true);
        _anim.SetBool("Jumping", false);
    }

    public override void UpdateState()
    {
        if(_input.GetHorizontalMoveInput() != 0)
            _player.TransitionToState(_player.walking);

        if (_input.RememberJumpInput())
        {
            TransitionToJump();
            return;
        }

        if (_onLadder == false)
        {
            _player.TransitionToState(_player.idle);
            return;
        }
        
        if (_input.HoldingClimb())
        {
            _player.velocity.y = _climbSpeed;
            _anim.speed = 1;
        }
        else if (_input.HoldingCrouch())
        {
            _player.velocity.y = -_climbSpeed;
            _anim.speed = 1;
            if (_player.isGrounded)
                _player.TransitionToState(_player.idle);
        }
        else
        {
            _player.velocity.y = 0;
            _anim.speed = 0;
        }
    }

    public override void FixedUpdateState()
    {
        _onLadder = LadderCheck();
    }

    private bool LadderCheck()
    {
        Collider2D[] results = Physics2D.OverlapCircleAll(_player.transform.position, .2f, 1 << 12);
        if (results != null && results.Length != 0)
        {
            return true;
        }

        return false;
    }
    
    public override void Exit()
    {
        _input.OnAttack -= TransitionToAttack;
        _anim.SetBool("Climbing Ladder", false);
        _anim.speed = 1;
    }

    private void TransitionToJump()
    {
        _player.isGrounded = true;
        _player.TransitionToState(_player.jump);
    }

    private void TransitionToAttack()
    {
        _player.TransitionToState(_player.attack);
    }
}
