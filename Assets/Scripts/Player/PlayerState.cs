using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class PlayerState
{
    protected PlayerBehaviour _player;
    
    public virtual void Awake(){}
    public virtual void Start(){}

    public virtual void Enter(){}
    public virtual void UpdateState(){}
    public virtual void FixedUpdateState(){}
    public virtual void Exit(){}

    public virtual void OnValidate(PlayerBehaviour player)
    {
        _player = player;
    }
}
