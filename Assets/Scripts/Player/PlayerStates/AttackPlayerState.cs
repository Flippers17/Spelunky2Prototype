using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackPlayerState : PlayerState
{
    [SerializeField] private float acionDelay = 0.4f;
    private float timer = 0;

    [SerializeField]
    private Vector2 _throwingVelocity = new Vector2(6, 2);
    
    //public override void Awake(){}
    //public override void Start(){}

    public override void Enter()
    {
        if(_player.currentHeldItem != null)
        {
            _player.currentHeldItem.ThrowItem(new Vector2(_throwingVelocity.x * _player.facingDirection, _throwingVelocity.y));
            _player.currentHeldItem = null;
            _stateMachine.TransitionToState(_stateMachine.idle);
            return;
        }

        if(_player.isGrounded)
            _player.velocity.x = 0;
        _anim.SetBool("Attacking", true);
        _anim.SetBool("Jumping", false);
        timer = 0;
    }

    public override void UpdateState()
    {
        if (timer < acionDelay)
            timer += Time.deltaTime;
        else
            _stateMachine.TransitionToState(_stateMachine.idle);

        if (_player.isGrounded)
            _player.velocity.y = -1;
        else
            _player.velocity.y -= _player.GetGravity() * Time.deltaTime;
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void Exit()
    {
        _anim.SetBool("Attacking", false);
        _player.DeactivateWhipHurtbox();
    }

}
