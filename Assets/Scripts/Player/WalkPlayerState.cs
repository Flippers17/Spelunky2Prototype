using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WalkPlayerState : PlayerState
{
    [SerializeField] private float _speed = 4f;
    private PlayerInputHandler _input;
    
    public override void Awake(){}
    public override void Start(){}
   
    public override void Enter()
    {
       
    }

    public override void UpdateState()
    {
        float walkInput = _input.GetHorizontalMoveInput();
        if (walkInput != 0)
            _player.velocity.x = _speed * walkInput;
        else
            _player.TransitionToState(_player.idle);

        CheckTransitions();
    }


    private void CheckTransitions()
    {
        if (_input.RememberJumpInput())
            TransitionToJump();
        else if (_input.HoldingRun())
            _player.TransitionToState(_player.running);
    }



    public override void FixedUpdateState()
    {
        if (!_player.isGrounded)
            _player.velocity.y -= _player.GetGravity() * Time.fixedDeltaTime;
        else
            _player.velocity.y = -1;
    }

    public override void Exit()
    {
        
    }

    private void TransitionToJump()
    {
        if (!_player.WithinCoyoteTime())
            return;

        _player.TransitionToState(_player.jump);
    }

    public override void OnValidate(PlayerBehaviour player)
    {
        base.OnValidate(player);
        _input = player.input;
    }
}
