using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsDelay : MonoBehaviour
{
    [SerializeField]
    private float _delay = 0.2f;

    private void OnEnable()
    {
        Time.timeScale = 0;
        StartCoroutine(CountDelay());
    }

    IEnumerator CountDelay()
    {
        yield return new WaitForSecondsRealtime(_delay);
        Time.timeScale = 1;
    }
}
