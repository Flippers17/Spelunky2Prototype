using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Door : MonoBehaviour
{
    public Transform enterPoint;

    [SerializeField] private EnterDoorEventPort enterDoorEventPort;

    [SerializeField] private Door targetDoor;

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

    private void EnterDoor(PlayerBehaviour player)
    {
        player.transform.position = enterPoint.position;
        enterDoorEventPort.InvokeOnEnterDoor(targetDoor);
    }
    
}
