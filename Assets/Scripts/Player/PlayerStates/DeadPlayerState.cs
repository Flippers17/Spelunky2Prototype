using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeadPlayerState : PlayerState
{
    public override void Enter()
    {
        _anim.SetBool("Dead", true);
    }

    public override void UpdateState()
    {
        if (!_player.isGrounded)
            _player.velocity.y -= _player.gravity * Time.deltaTime;
        else
            _player.velocity.y = -1;
    }
}
