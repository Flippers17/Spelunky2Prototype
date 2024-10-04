using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

[System.Serializable]
public class EnterDoorPlayerState : PlayerState
{
    //[SerializeField, HideInInspector]
    private EnterDoorEventPort EnterDoorEvent;
    [SerializeField] private float waitTime = 1f;
    private float timer = 0;
    
    public override void Awake(PlayerBehaviour player, PlayerStateMachine stateMachine)
    {
        base.Awake(player, stateMachine);
        EnterDoorEvent = player.EnterDoorEvent;
    }

    //public override void Start(){}
   
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
            _stateMachine.TransitionToState(_stateMachine.idle);
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void Exit()
    {
        if(EnterDoorEvent.targetDoor != null)
            _player.transform.position = EnterDoorEvent.targetDoor.enterPoint.position;
        EnterDoorEvent.InvokeOnExitDoor();
    }

}
