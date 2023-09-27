using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public GameManager instance;

    [Header("Score")]
    public int score = 0;
    public ScoreEventPort scoreEvent;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this);
    }

    private void OnEnable()
    {
        scoreEvent.OnGainScore += IncreaseScore;
    }

    private void OnDisable()
    {
        scoreEvent.OnGainScore -= IncreaseScore;
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
    }
}
