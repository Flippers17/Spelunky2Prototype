using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    [SerializeField, Range(0.01f, 1)]
    private float _lerpSpeedX = 0.5f;
    [SerializeField, Range(0.01f, 1)]
    private float _lerpSpeedY = 0.02f;

    

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = _target.localToWorldMatrix.GetPosition();

        targetPos.x = transform.position.x * (1- _lerpSpeedX) + targetPos.x * _lerpSpeedX;
        targetPos.y = transform.position.y * (1 - _lerpSpeedY) + targetPos.y * _lerpSpeedY;
        targetPos.z = -10;
        transform.position = targetPos;
    }
}
