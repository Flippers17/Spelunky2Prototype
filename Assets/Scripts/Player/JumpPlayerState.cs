using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JumpPlayerState : PlayerState
{
    [SerializeField] private float jumpHeight = 1f;
    private PlayerInputHandler _input;
    [SerializeField] private float acionDelay = 0.1f;
    private float _timeSinceJumped = 0f;
    
    public override void Awake(){}
    public override void Start(){}

    public override void Enter()
    {
        _timeSinceJumped = 0;
        _player.velocity.y = Mathf.Sqrt(2f * jumpHeight * _player.gravity);
    }

    public override void UpdateState()
    {
        if(_timeSinceJumped < acionDelay)
            _timeSinceJumped += Time.deltaTime;
        else
            _player.TransitionToState(_player.idle);
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void Exit(){}

    public override void OnValidate(PlayerBehaviour player)
    {
        base.OnValidate(player);
        _input = player.input;
    }
}
