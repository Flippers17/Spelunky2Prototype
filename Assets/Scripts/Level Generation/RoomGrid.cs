using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGrid : MonoBehaviour
{
    [SerializeField]
    public Vector2Int _roomSize = new Vector2Int(18, 10);
    [SerializeField]
    public Vector2Int _gridSize = new Vector2Int(6, 4);

    [SerializeField]
    public Vector2Int bottomLeftCorner = new Vector2Int(0, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Returns what tile the top left corner of the given room is.
    /// </summary>
    /// <param name="coordinates"> Coordinates in the grid of rooms, (0, 0) is bottom left room</param>
    /// <returns></returns>
    public Vector2Int GetRoomCorner(Vector2Int coordinates)
    {
        return bottomLeftCorner + coordinates * _roomSize;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere((Vector2)bottomLeftCorner, .5f);

        Vector3 roomSizeConverted = new Vector3(_roomSize.x, _roomSize.y, 1);

        Gizmos.color = Color.green;

        for(int i = 0; i < _gridSize.x; i++)
        {
            for(int j = 0; j < _gridSize.y; j++)
            {
                Gizmos.DrawWireCube(bottomLeftCorner + new Vector2(_roomSize.x * i, _roomSize.y * j) + new Vector2(_roomSize.x/2, _roomSize.y/2), roomSizeConverted);
            }
        }
    }
}
