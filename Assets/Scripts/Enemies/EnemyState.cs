using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class EnemyState
{
    protected SnakeBehaviour _enemy;
    protected Animator _anim;

    public virtual void Awake(SnakeBehaviour enemy)
    {
        if(_enemy != null)
            return;

        _enemy = enemy;
        _anim = enemy.anim;
    }
    public virtual void Start() { }

    public virtual void Enter() { }
    public virtual void UpdateState() { }
    public virtual void FixedUpdateState() { }
    public virtual void Exit() { }

    public virtual void OnValidate(SnakeBehaviour enemy)
    {
        _enemy = enemy;
        _anim = enemy.anim;
    }
}
