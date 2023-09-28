using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    public Transform enterPoint;

    [SerializeField] protected EnterDoorEventPort enterDoorEventPort;

    [SerializeField] protected Door targetDoor;


    private void OnTriggerStay2D(Collider2D other)
    {
        if(targetDoor == null)
            return;
        
        if (other.TryGetComponent(out PlayerBehaviour player))
        {
            if (player.input.HoldingClimb() && player.isGrounded)
            {
                EnterDoor(player);
            }
        }
    }

    protected virtual void EnterDoor(PlayerBehaviour player)
    {
        player.transform.position = enterPoint.position;
        enterDoorEventPort.InvokeOnEnterDoor(targetDoor);
    }
    
}
