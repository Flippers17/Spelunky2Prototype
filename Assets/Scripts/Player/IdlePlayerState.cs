using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IdlePlayerState : PlayerState
{
    private PlayerInputHandler _input;
    
    public override void Awake(){}
    public override void Start(){}

    public override void Enter()
    {
        _player.velocity.x = 0;
        _input.OnJump += TransitionToJump;
    }

    public override void UpdateState()
    {
        if(_input.GetHorizontalMoveInput() != 0)
            _player.TransitionToState(_player.walking);
        
    }

    public override void FixedUpdateState()
    {
        if (!_player.IsGrounded())
            _player.velocity.y -= _player.GetGravity() * Time.fixedDeltaTime;
        else
            _player.velocity.y = -1;
    }

    public override void Exit()
    {
        _input.OnJump += TransitionToJump;
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
