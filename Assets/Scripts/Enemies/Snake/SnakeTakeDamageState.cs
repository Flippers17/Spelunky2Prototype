using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTakeDamageState : TakeDamageEnemeyState
{
    private SnakeBehaviour _snake;

    public override void Awake(EnemyBehaviour enemy)
    {
        base.Awake(enemy);
        if(_snake)
            return;
        _snake = enemy as SnakeBehaviour;
    }

    protected override void TransitionFromState()
    {
        _snake.TransitionToState(_snake.walking);
    }

    public override void OnValidate(EnemyBehaviour enemy)
    {
        base.OnValidate(enemy);
        _snake = enemy as SnakeBehaviour;
    }
}
