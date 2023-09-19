using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LedgeGrabPlayerState : PlayerState
{
    private PlayerInputHandler _input;
    
    public override void Awake(){}
    public override void Start(){}
   
    public override void Enter()
    {
        _player.velocity = new Vector2(1 * _player.facingDirection, 0);
    }

    public override void UpdateState()
    {
        CheckTransitions();
    }


    private void CheckTransitions()
    {
        if(_input.HoldingCrouch() && _input.HoldingJump())
            _player.TransitionToState(_player.idle);
        else if (_input.RememberJumpInput())
            TransitionToJump();
    }



    public override void FixedUpdateState()
    {
        //if (!_player.isGrounded)
        //    _player.velocity.y -= _player.GetGravity() * Time.fixedDeltaTime;
        //else
        //    _player.velocity.y = -1;
    }

    public override void Exit()
    {
        
    }

    private void TransitionToJump()
    {
        _player.TransitionToState(_player.jump);
    }

    public override void OnValidate(PlayerBehaviour player)
    {
        base.OnValidate(player);
        _input = player.input;
    }
}
