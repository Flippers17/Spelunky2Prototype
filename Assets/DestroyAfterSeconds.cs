using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField] private float _seconds = 3;
    
    private void Awake()
    {
        Destroy(this.gameObject, _seconds);
    }
}
