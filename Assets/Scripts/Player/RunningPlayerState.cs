using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RunningPlayerState : PlayerState
{
    [SerializeField] private float _speed = 6f;
    [SerializeField] private float _acceleration = 3f;
    private PlayerInputHandler _input;

    public override void Awake() { }
    public override void Start() { }

    public override void Enter()
    {

    }

    public override void UpdateState()
    {
        float walkInput = _input.GetHorizontalMoveInput();
        float targetVelocity = _speed * walkInput;
        if (walkInput != 0)
        {
            if (Mathf.Abs(_player.velocity.x) < Mathf.Abs(targetVelocity))
                _player.velocity.x += Time.deltaTime * _acceleration * walkInput;
            else
                _player.velocity.x = targetVelocity;
        }
        else
            _player.TransitionToState(_player.idle);

        CheckTransitions();
    }


    private void CheckTransitions()
    {
        if (_input.RememberJumpInput())
            TransitionToJump();
        else if (!_input.HoldingRun())
            _player.TransitionToState(_player.walking);
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
