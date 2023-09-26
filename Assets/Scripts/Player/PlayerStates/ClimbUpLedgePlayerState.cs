using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbUpLedgePlayerState : PlayerState
{
    [SerializeField] private float _speed = 4f;


    public override void Awake() { }
    public override void Start() { }

    public override void Enter()
    {

    }

    public override void UpdateState()
    {
        CheckTransitions();
    }


    private void CheckTransitions()
    {
        if (OnLedge())
            _player.TransitionToState(_player.idle);
    }


    private bool AboveLedge()
    {
        Vector2 playerPos = _player.transform.position;

        return !Physics2D.Raycast(playerPos - Vector2.up * 0.5f, Vector2.right * _player.facingDirection, .7f,_player.groundMask);
    }

    private bool OnLedge()
    {
        Vector2 playerPos = _player.transform.position;

        return Physics2D.Raycast(playerPos - Vector2.right * (_player.facingDirection * 0.5f), Vector2.down, .7f, _player.groundMask);
    }

    public override void FixedUpdateState()
    {
        if (!AboveLedge())
        {
            _player.velocity = new Vector2(0, _speed);
        }
        else
            _player.velocity = new Vector2(_speed * _player.facingDirection, 0);
    }

    public override void Exit()
    {
        
    }

}
