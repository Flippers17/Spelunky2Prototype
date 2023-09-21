using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackPlayerState : PlayerState
{
    [SerializeField] private float acionDelay = 0.4f;
    private float timer = 0;
    
    public override void Awake(){}
    public override void Start(){}

    public override void Enter()
    {
        _player.velocity.x = 0;
        _anim.SetBool("Attacking", true);
        timer = 0;
    }

    public override void UpdateState()
    {
        if (timer < acionDelay)
            timer += Time.deltaTime;
        else
            _player.TransitionToState(_player.idle);
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void Exit()
    {
        _anim.SetBool("Attacking", false);
        _player.DeactivateWhipHurtbox();
    }

    public override void OnValidate(PlayerBehaviour player)
    {
        base.OnValidate(player);
    }
}
