using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Score Event Port", fileName = "New Score Event Port")]
public class ScoreEventPort : ScriptableObject
{
    public UnityAction<int> OnGainScore = delegate { };
    public UnityAction<int> OnUpdateScore = delegate { };

    public void IncreaseScore(int amount)
    {
        OnGainScore?.Invoke(amount);
    }

    public void UpdateScore(int amount)
    {
        OnUpdateScore?.Invoke(amount);
    }
}
