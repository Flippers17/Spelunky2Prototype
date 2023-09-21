using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LedgeGrabPlayerState : PlayerState
{
    
    public override void Awake(){}
    public override void Start(){}
   
    public override void Enter()
    {
        _anim.SetBool("Jumping", false);
        _anim.SetBool("Ledge Grabing", true);
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
        
    }

    public override void Exit()
    {
        _anim.SetBool("Ledge Grabing", false);
    }

    private void TransitionToJump()
    {
        _player.TransitionToState(_player.jump);
    }

    public override void OnValidate(PlayerBehaviour player)
    {
        base.OnValidate(player);
    }
}
