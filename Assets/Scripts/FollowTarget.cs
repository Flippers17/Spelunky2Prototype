using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    public bool interpolatePosition = true;
    [SerializeField, Range(0.01f, 1)]
    private float _lerpSpeedX = 0.5f;
    [SerializeField, Range(0.01f, 1)]
    private float _lerpSpeedY = 0.02f;

    [SerializeField, Space(10)] private EnterDoorEventPort EnterDoorEvent;

    private void OnEnable()
    {
        if (EnterDoorEvent)
        {
            EnterDoorEvent.OnEnterDoor += OnEnterDoor;
            EnterDoorEvent.OnExitDoor += OnExitDoor;
        }
    }

    private void OnDisable()
    {
        if (EnterDoorEvent)
        {
            EnterDoorEvent.OnEnterDoor -= OnEnterDoor;
            EnterDoorEvent.OnExitDoor -= OnExitDoor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = _target.localToWorldMatrix.GetPosition();

        if (interpolatePosition)
        {
            targetPos.x = transform.position.x * (1- _lerpSpeedX) + targetPos.x * _lerpSpeedX;
            targetPos.y = transform.position.y * (1 - _lerpSpeedY) + targetPos.y * _lerpSpeedY;
        }
        targetPos.z = -10;
        transform.position = targetPos;
    }

    private void OnEnterDoor()
    {
        interpolatePosition = false;
    }

    private void OnExitDoor()
    {
        interpolatePosition = true;
    }
}
