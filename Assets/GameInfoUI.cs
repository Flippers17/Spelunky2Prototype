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
    [SerializeField]
    private PlayerHealthSO _playerHealthSO;
    [SerializeField]
    private TextMeshProUGUI _healthText;
    [SerializeField]
    private ScoreEventPort _bombPickupEvent;
    [SerializeField]
    private TextMeshProUGUI _bombsText;

    [SerializeField]
    private GameObject _deathScreen;

    private void OnEnable()
    {
        if (_scoreEvent)
        {
            _scoreEvent.OnUpdateScore += UpdateScore;
        }
        
        if (_bombPickupEvent)
        {
            _bombPickupEvent.OnUpdateScore += UpdateBombs;
        }

        if (_playerHealthSO)
        {
            _playerHealthSO.OnChangeHealth += UpdateHealth;
            _playerHealthSO.OnDie += OnPlayerDeath;
        }
        
        if(GameManager.instance)
            GameManager.instance.UpdateInfo();
    }

    private void OnDisable()
    {
        if(_scoreEvent)
            _scoreEvent.OnUpdateScore -= UpdateScore;

        if (_bombPickupEvent)
        {
            _bombPickupEvent.OnUpdateScore -= UpdateBombs;
        }
        
        if (_playerHealthSO)
        {
            _playerHealthSO.OnChangeHealth -= UpdateHealth;
            _playerHealthSO.OnDie -= OnPlayerDeath;
        }
    }

    private void UpdateScore(int amount)
    {
        _scoreText.text = "Score: " + amount;
    }
    
    private void UpdateHealth(int amount)
    {
        _healthText.text = "X " + amount;
    }
    
    private void UpdateBombs(int amount)
    {
        _bombsText.text = "X " + amount;
    }

    private void OnPlayerDeath()
    {
        _deathScreen.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void RestartGame()
    {
        GameManager.instance.StartRun();
    }
    
    public void GoToMainMenu()
    {
        GameManager.instance.LoadLevelAtIndex(0);
    }
}
