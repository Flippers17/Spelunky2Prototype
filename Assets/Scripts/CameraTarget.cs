using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{

    [SerializeField]
    private Transform _target;
    [SerializeField]
    private PlayerInputHandler _input;

    [SerializeField, Space(10)]
    private float lookingDownOffset =  -2;
    [SerializeField]
    private float lookingUpOffset = 2;

    private float _timeLookingDown = 0f;
    private float _timeLookingUp = 0f;

    private Vector2 _offset = Vector2.zero;

    private void OnValidate()
    {
        if (_input == null)
            if (!TryGetComponent(out _input))
                Debug.LogWarning("CameraTarget is missing PlayerInputHandler reference", this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.HoldingCrouch())
        {
            if(_timeLookingDown < 1)
                _timeLookingDown += Time.deltaTime;
            _timeLookingUp = 0;
        }
        else if (_input.HoldingClimb())
        {
            if(_timeLookingUp < 1)
                _timeLookingUp += Time.deltaTime;
            _timeLookingDown = 0;
        }
        else
        {
            _timeLookingDown = 0;
            _timeLookingUp = 0;
        }

        if(_timeLookingDown > 1)
        {
            _offset.y = lookingDownOffset;
        }
        else if( _timeLookingUp > 1)
        {
            _offset.y = lookingUpOffset;
        }
        else
        {
            _offset.y = 0;
        }

        _target.localPosition = _offset;
    }
}
