using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

[System.Serializable]
public class EnterDoorPlayerState : PlayerState
{
    private EnterDoorEventPort EnterDoorEvent;
    [SerializeField] private float waitTime = 1f;
    private float timer = 0;
    
    public override void Awake(){}
    public override void Start(){}
   
    public override void Enter()
    {
        _player.velocity = Vector2.zero;
        timer = 0;
    }

    public override void UpdateState()
    {
        if (timer < waitTime)
            timer += Time.deltaTime;
        else
            _player.TransitionToState(_player.idle);
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void Exit()
    {
        _player.transform.position = EnterDoorEvent.targetDoor.enterPoint.position;
        EnterDoorEvent.InvokeOnExitDoor();
    }

    public override void OnValidate(PlayerBehaviour player)
    {
        base.OnValidate(player);
        EnterDoorEvent = player.EnterDoorEvent;
    }
}
