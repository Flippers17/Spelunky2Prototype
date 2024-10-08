using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TakeDamagePlayerState : PlayerState
{
    [SerializeField]
    private float stunnedTime = 1f;
    private float timer = 0f;

    //public override void Awake() { }
    //public override void Start() { }

    public override void Enter()
    {
        _anim.SetBool("Jumping", false);
        _anim.SetBool("Taking Damage", true);
        AudioManager.instance.Play("Hurt");
        timer = 0;
    }

    public override void UpdateState()
    {
        if(timer < stunnedTime)
            timer += Time.deltaTime;

        if (timer > stunnedTime && _player.isGrounded)
            _stateMachine.TransitionToState(_stateMachine.idle);
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
        _anim.SetBool("Taking Damage", false);
        _player.timeSinceDamaged = 0;
    }

}
