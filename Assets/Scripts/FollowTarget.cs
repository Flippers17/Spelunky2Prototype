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

    [SerializeField]
    private Vector2 _bottomLeftBoundary = Vector2.zero;
    [SerializeField]
    private Vector2 _topRightBoundary = Vector2.zero;

    private Vector2 cameraSize = Vector2.zero;

    [SerializeField, Space(10)] private EnterDoorEventPort EnterDoorEvent;

    private void OnEnable()
    {
        if (EnterDoorEvent)
        {
            EnterDoorEvent.OnEnterDoor += OnEnterDoor;
            EnterDoorEvent.OnExitDoor += OnExitDoor;
        }
        cameraSize.y = Camera.main.orthographicSize * 2f;
        cameraSize.x = cameraSize.y * Camera.main.aspect;
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
            targetPos.x = transform.position.x * (1 - _lerpSpeedX) + targetPos.x * _lerpSpeedX;
            targetPos.y = transform.position.y * (1 - _lerpSpeedY) + targetPos.y * _lerpSpeedY;
        }
        targetPos.z = -10;

        if(targetPos.x - cameraSize.x/2 < _bottomLeftBoundary.x)
            targetPos.x = _bottomLeftBoundary.x + cameraSize.x / 2;
        else if (targetPos.x + cameraSize.x / 2 > _topRightBoundary.x)
            targetPos.x = _topRightBoundary.x - cameraSize.x / 2;

        if (targetPos.y - cameraSize.y / 2 < _bottomLeftBoundary.y)
            targetPos.y = _bottomLeftBoundary.y + cameraSize.y / 2;
        else if (targetPos.y + cameraSize.y / 2 > _topRightBoundary.y)
            targetPos.y = _topRightBoundary.y - cameraSize.y / 2;

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_bottomLeftBoundary, new Vector3(_bottomLeftBoundary.x, _topRightBoundary.y));
        Gizmos.DrawLine(new Vector3(_bottomLeftBoundary.x, _topRightBoundary.y), _topRightBoundary);

        Gizmos.DrawLine(_bottomLeftBoundary, new Vector3(_topRightBoundary.x, _bottomLeftBoundary.y));
        Gizmos.DrawLine(new Vector3(_topRightBoundary.x, _bottomLeftBoundary.y), _topRightBoundary);
    }
}
