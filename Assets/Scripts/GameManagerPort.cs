using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameManagerPort : ScriptableObject
{
    public void StartRun()
    {
        GameManager.instance.StartRun();
    }

    public void QuitGame()
    {
        GameManager.instance.QuitGame();
    }
}
