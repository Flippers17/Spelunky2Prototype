using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameInfoUI : MonoBehaviour
{
    [SerializeField]
    private ScoreEventPort _scoreEvent;
    [SerializeField]
    private TextMeshProUGUI _scoreText;

    private void OnEnable()
    {
        _scoreEvent.OnUpdateScore += UpdateScore;
    }

    private void OnDisable()
    {
        _scoreEvent.OnUpdateScore -= UpdateScore;
    }

    private void UpdateScore(int amount)
    {
        _scoreText.text = "Score: " + amount;
    }
}
