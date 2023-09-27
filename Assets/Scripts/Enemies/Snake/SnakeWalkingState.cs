using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SnakeWalkingState : WalkingEnemyState
{
    private SnakeBehaviour _snake;

    public override void Awake(EnemyBehaviour enemy)
    {
        base.Awake(enemy);
        if(_snake)
            return;
        
        _snake = enemy as SnakeBehaviour;
    }

    public override void OnValidate(EnemyBehaviour enemy)
    {
        base.OnValidate(enemy);
        _snake = enemy as SnakeBehaviour;
    }
}
