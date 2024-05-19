using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClimbDownLedgePlayerState : PlayerState
{
    [SerializeField] private float _speed = 4f;


    //public override void Awake() { }
    //public override void Start() { }

    public override void Enter()
    {
        _player.facingDirection *= -1;
        _anim.SetBool("Climbing Down", true);
    }

    public override void UpdateState()
    {
        CheckTransitions();
    }


    private void CheckTransitions()
    {
        if (LedgeGrabDetection())
            _player.TransitionToState(_player.ledgeGrab);
    }


    private bool LedgeGrabDetection()
    {
        Vector2 playerPos = _player.transform.position;

        return !Physics2D.Raycast(playerPos + Vector2.up * 0.4f, Vector2.right * _player.facingDirection, .7f,
                   _player.groundMask) && Physics2D.Raycast(playerPos + Vector2.up * 0.2f, Vector2.right * _player.facingDirection, 0.7f, _player.groundMask);
    }


    public override void FixedUpdateState()
    {
        if (Physics2D.Raycast((Vector2)_player.transform.position + Vector2.right * (_player.facingDirection * 0.4f), Vector2.down, 0.6f, _player.groundMask))
        {
            _player.velocity = new Vector2(_speed * -_player.facingDirection, -1);
        }
        else
        {
            _player.velocity = new Vector2(0, -3);
        }
    }

    public override void Exit()
    {
        _anim.SetBool("Climbing Down", false);
    }

}
