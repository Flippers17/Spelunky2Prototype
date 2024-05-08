using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private RoomGrid _roomGrid;

    [SerializeField]
    private Tilemap _groundTiles;
    [SerializeField]
    private Tilemap _indestructibleTiles;

    [SerializeField]
    private List<RoomTemplate> _rooms = new List<RoomTemplate>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateLevel()
    {
        Vector2Int roomSize = _roomGrid._roomSize;

        //CopyRoomToPlace(_roomGrid._topLeftCorner, roomSize, _rooms[0]);
        //return;
        for (int i = 0; i < _roomGrid._gridSize.x; i++)
        {
            for(int j = 0; j < _roomGrid._gridSize.y; j++)
            {
                CopyRoomToPlace(new Vector2Int(i, j), roomSize, _rooms[0]);
            }
        }
    }


    private void CopyRoomToPlace(Vector2Int roomCoordinates, Vector2Int roomSize, RoomTemplate template)
    {

        for(int x = 0; x < roomSize.x; x++)
        {
            for(int y = 0; y < roomSize.y; y++)
            {
                Vector3Int currentPos = new Vector3Int(x, y);
                _groundTiles.SetTile(currentPos + (Vector3Int)_roomGrid.GetRoomCorner(roomCoordinates), template.groundTiles.GetTile(currentPos + (Vector3Int)template.bottomLeftCorner));
            }
        }
    }
}
