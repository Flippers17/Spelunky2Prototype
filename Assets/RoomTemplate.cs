using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomTemplate : MonoBehaviour
{

    public RoomTags tags;

    public Tilemap groundTiles;
    public Tilemap indestructableTiles;

    public Vector2Int bottomLeftCorner = new Vector2Int(-9, -5);


    private void Start()
    {
        var flags = RoomTags.EntranceWest | RoomTags.EntranceEast;

        Debug.Log(flags.HasFlag(tags));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere((Vector2)bottomLeftCorner, .5f);
    }

}

[System.Flags]
public enum RoomTags : int
{
    None = 0,
    EntranceWest = 1 << 0,
    EntranceEast = 1 << 1,
    EntranceNorth = 1 << 2,
    EntranceSouth = 1 << 3,
    MainExit = 1 << 4,
    Optional = 1 << 5,
}
