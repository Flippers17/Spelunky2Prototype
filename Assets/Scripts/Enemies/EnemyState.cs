using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class EnemyState
{
    protected EnemyBehaviour _enemy;
    protected Animator _anim;

    public virtual void Awake(EnemyBehaviour enemy)
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

    public virtual void OnValidate(EnemyBehaviour enemy)
    {
        _enemy = enemy;
        _anim = enemy.anim;
    }
}
