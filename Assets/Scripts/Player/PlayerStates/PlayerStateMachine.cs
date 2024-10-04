using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStateMachine
{
    public PlayerState currentState;

    public IdlePlayerState idle = new IdlePlayerState();
    public WalkPlayerState walking = new WalkPlayerState();
    public RunningPlayerState running = new RunningPlayerState();
    public CrouchPlayerState crouching = new CrouchPlayerState();
    public JumpPlayerState jump = new JumpPlayerState();
    public LedgeGrabPlayerState ledgeGrab = new LedgeGrabPlayerState();
    public ClimbDownLedgePlayerState climbDown = new ClimbDownLedgePlayerState();
    public ClimbUpLedgePlayerState climbUp = new ClimbUpLedgePlayerState();
    public TakeDamagePlayerState takeDamage = new TakeDamagePlayerState();
    public AttackPlayerState attack = new AttackPlayerState();
    public EnterDoorPlayerState enterDoor = new EnterDoorPlayerState();
    public DeadPlayerState dead = new DeadPlayerState();
    public ClimbLadderPlayerState climbLadder = new ClimbLadderPlayerState();


    public void OnDisable()
    {
        currentState.Exit();
    }


    public void Start()
    {
        idle.Start();
        walking.Start();
        running.Start();
        crouching.Start();
        jump.Start();
        ledgeGrab.Start();
        climbDown.Start();
        climbUp.Start();
        takeDamage.Start();
        attack.Start();
        enterDoor.Start();
        dead.Start();
        climbLadder.Start();

        currentState = idle;
        currentState.Enter();
    }


    public void Awake(PlayerBehaviour playerBehaviour)
    {
        idle.Awake(playerBehaviour, this);
        walking.Awake(playerBehaviour, this);
        running.Awake(playerBehaviour, this);
        crouching.Awake(playerBehaviour , this);
        jump.Awake(playerBehaviour, this);
        ledgeGrab.Awake(playerBehaviour, this);
        climbDown.Awake(playerBehaviour, this);
        climbUp.Awake(playerBehaviour, this);
        takeDamage.Awake(playerBehaviour, this);
        attack.Awake(playerBehaviour, this);
        enterDoor.Awake(playerBehaviour, this);
        dead.Awake(playerBehaviour, this);
        climbLadder.Awake(playerBehaviour, this);
    }

    public void OnUpdate()
    {
        if (currentState != null)
            currentState.UpdateState();
    }

    public void OnFixedUpdate()
    {
        if (currentState != null)
            currentState.FixedUpdateState();
    }

    public void TransitionToState(PlayerState targetState)
    {
        currentState.Exit();
        currentState = targetState;
        currentState.Enter();
    }
}
