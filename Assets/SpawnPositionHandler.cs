using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPositionHandler : MonoBehaviour
{
    [SerializeField]
    private Vector2 _offset;

    private void OnEnable()
    {
        LevelGenerator.OnLevelGenerated += PlaceAtSpawnPosition;
    }

    private void OnDisable()
    {
        LevelGenerator.OnLevelGenerated -= PlaceAtSpawnPosition;
    }


    private void PlaceAtSpawnPosition()
    {
        transform.position = (Vector2)LevelGenerator.PlayerStartPos + _offset;
    }
}
