using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SnakeWalkingState : WalkingEnemyState
{
    private SnakeBehaviour _snake;
    
    public override void OnValidate(EnemyBehaviour enemy)
    {
        base.OnValidate(enemy);
        _snake = enemy as SnakeBehaviour;
    }
}
