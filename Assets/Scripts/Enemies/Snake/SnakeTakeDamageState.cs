using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTakeDamageState : TakeDamageEnemeyState
{
    private SnakeBehaviour _snake;
    
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
