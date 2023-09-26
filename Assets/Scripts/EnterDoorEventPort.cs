using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Enter Door Event Port", fileName = "EnterDoorEventPort")]
public class EnterDoorEventPort : ScriptableObject
{
    public UnityAction OnEnterDoor = () => { };
    public UnityAction OnExitDoor = () => { };
    public Door targetDoor;

    public void InvokeOnEnterDoor(Door TargetDoor)
    {
        if(!TargetDoor)
            return;

        targetDoor = TargetDoor;
        OnEnterDoor?.Invoke();
    }

    public void InvokeOnExitDoor()
    {
        OnExitDoor?.Invoke();
    }
}
