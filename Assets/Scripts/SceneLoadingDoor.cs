using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadingDoor : Door
{
    [SerializeField]
    private int targetLevelIndex = 1;
    [SerializeField]
    private string targetLevelName = "";

    [SerializeField]
    private bool useNameForLevelLoading = false;

    [SerializeField]
    private bool loadNextLevel = false;
    private bool isLoadingScene = false;

    private void OnValidate()
    {
        targetDoor = this;
    }

    protected override void EnterDoor(PlayerBehaviour player)
    {
        base.EnterDoor(player);
        enterDoorEventPort.OnExitDoor += TriggerSceneLoad;
    }

    private void TriggerSceneLoad()
    {
        if (isLoadingScene)
            return;

        isLoadingScene = true;

        if (loadNextLevel)
            GameManager.instance.LoadNextLevel();
        else if(!useNameForLevelLoading)
            GameManager.instance.LoadLevelAtIndex(targetLevelIndex);
        else
            GameManager.instance.LoadLevelByName(targetLevelName);
    }
}
