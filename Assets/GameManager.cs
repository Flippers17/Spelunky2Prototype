using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static GameManager instance;

    [Header("Score")]
    public int score = 0;
    public ScoreEventPort scoreEvent;
    public PlayerHealthSO playerHealthSO;

    private int currentLevelIndex = 2;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            playerHealthSO.ResetHealth();
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
        scoreEvent.UpdateScore(score);
    }

    public void LoadNextLevel()
    {
        currentLevelIndex++;
        SceneManager.LoadScene(currentLevelIndex);
    }
    
    public void LoadLevelAtIndex(int index)
    {
        SceneManager.LoadScene(index);   
    }

    public void LoadLevelByName(string name)
    {
        SceneManager.LoadScene(name);
    }

    
    public void StartRun()
    {
        playerHealthSO.ResetHealth();
        score = 0;
        scoreEvent.UpdateScore(score);
        currentLevelIndex = 2;
        SceneManager.LoadScene(2);
    }

    public void UpdateInfo()
    {
        playerHealthSO.ChangeHealth(0);
        scoreEvent.UpdateScore(score);
    }
}
