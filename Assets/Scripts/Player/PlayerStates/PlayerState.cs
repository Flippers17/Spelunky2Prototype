using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class PlayerState
{
    [SerializeField, HideInInspector]
    protected PlayerBehaviour _player;
    [SerializeField, HideInInspector]
    protected PlayerInputHandler _input;
    [SerializeField, HideInInspector]
    protected Animator _anim;
    
    public virtual void Awake(){}
    public virtual void Start(){}

    public virtual void Enter(){}
    public virtual void UpdateState(){}
    public virtual void FixedUpdateState(){}
    public virtual void Exit(){}

    public virtual void OnValidate(PlayerBehaviour player)
    {
        _player = player;
        _input = player.input;
        _anim = player.anim;
    }
}
