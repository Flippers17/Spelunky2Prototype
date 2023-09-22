using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    [SerializeField, Range(0.01f, 1)]
    private float _lerpSpeed = 0.2f;

    

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = Vector3.Lerp((Vector2)transform.position, _target.localToWorldMatrix.GetPosition(), _lerpSpeed);
        targetPos.z = -10;
        transform.position = targetPos;
    }
}
