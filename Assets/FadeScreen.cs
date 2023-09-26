using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    [SerializeField] private EnterDoorEventPort EnterDoorEvent;
    [SerializeField] private Animator _anim;

    private void OnEnable()
    {
        EnterDoorEvent.OnEnterDoor += StartFade;
        EnterDoorEvent.OnExitDoor += EndFade;
    }

    private void OnDisable()
    {
        EnterDoorEvent.OnEnterDoor -= StartFade;
        EnterDoorEvent.OnExitDoor -= EndFade;
    }

    private void StartFade()
    {
        _anim.ResetTrigger("EndFade");
        _anim.SetTrigger("StartFade");
    }

    private void EndFade()
    {
        _anim.ResetTrigger("StartFade");
        _anim.SetTrigger("EndFade");
    }
}
