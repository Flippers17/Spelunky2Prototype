using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JumpPlayerState : PlayerState
{
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float acionDelay = 0.1f;
    private float _timeSinceJumped = 0f;
    
    //public override void Awake(){}
    //public override void Start(){}

    public override void Enter()
    {
        _timeSinceJumped = 0;
        _player.velocity.y = Mathf.Sqrt(2f * jumpHeight * _player.gravity);
        _anim.SetBool("Jumping", true);
    }

    public override void UpdateState()
    {
        if (_timeSinceJumped < acionDelay)
            _timeSinceJumped += Time.deltaTime;
        else if (_input.GetHorizontalMoveInput() == 0)
            _stateMachine.TransitionToState(_stateMachine.idle);
        else if (_input.HoldingRun())
            _stateMachine.TransitionToState(_stateMachine.running);
        else
            _stateMachine.TransitionToState(_stateMachine.walking);
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void Exit()
    {

    }
}
