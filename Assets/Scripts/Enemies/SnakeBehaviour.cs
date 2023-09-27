using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class SnakeBehaviour : EnemyBehaviour
{
    public SnakeWalkingState walking = new SnakeWalkingState();
    public SnakeTakeDamageState takingDamage = new SnakeTakeDamageState();

    protected override void OnValidate()
    {
        base.OnValidate();
        walking.OnValidate(this);
        takingDamage.OnValidate(this);
    }

    private void Awake()
    {
        walking.Awake(this);
        takingDamage.Awake(this);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        currentState = walking;

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
