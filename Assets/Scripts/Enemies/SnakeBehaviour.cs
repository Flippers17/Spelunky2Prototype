using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class SnakeBehaviour : EnemyBehaviour
{
    public IdleEnemyState idle = new IdleEnemyState();
    public WalkingEnemyState walking = new WalkingEnemyState();
    public TakeDamageEnemeyState takingDamage = new TakeDamageEnemeyState();

    protected override void OnValidate()
    {
        base.OnValidate();
        idle.OnValidate(this);
        walking.OnValidate(this);
        takingDamage.OnValidate(this);
    }

    private void Awake()
    {
        idle.Awake(this);
        walking.Awake(this);
        takingDamage.Awake(this);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        currentState = walking;
        currentState.Enter();

        idle.Start();
        walking.Start();
        takingDamage.Start();
    }

    public override void TakeDamage(int damage, Vector2 knockback)
    {
        base.TakeDamage(damage, knockback);
        if(currentState != takingDamage)
            TransitionToState(takingDamage);
    }
}
