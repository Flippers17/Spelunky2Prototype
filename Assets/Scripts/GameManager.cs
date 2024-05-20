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
    [SerializeField]
    private int maxLevels = 16;

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
        Time.timeScale = 0;
        currentLevelIndex++;

        if(currentLevelIndex < maxLevels)
            SceneManager.LoadScene(2);
        else
        {
            SceneManager.LoadScene(3);
            Time.timeScale = 1;
        }

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
        currentLevelIndex = 0;
        LoadNextLevel();
    }

    public void UpdateInfo()
    {
        playerHealthSO.ChangeHealth(0);
        scoreEvent.UpdateScore(score);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
