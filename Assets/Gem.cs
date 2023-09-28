using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField]
    private int _scoreGain = 200;
    [SerializeField]
    private ScoreEventPort _scoreEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _scoreEvent.IncreaseScore(_scoreGain);
            Destroy(this.gameObject);
        }
    }
}
