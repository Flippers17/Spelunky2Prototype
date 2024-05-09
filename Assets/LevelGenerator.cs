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

        Vector2Int startRoom = new Vector2Int(Random.Range(0, _roomGrid._gridSize.x), _roomGrid._gridSize.y - 1);
        RoomTags currentTags = RoomTags.None;

        List<Vector2Int> roomPath = new List<Vector2Int>();
        roomPath.Add(startRoom);
        CalculatePath(startRoom, ref roomPath);
        
        for(int i = 0; i < roomPath.Count; i++)
        {
            currentTags = RoomTags.None;

            if(i - 1 >= 0)
            {
                currentTags |= GetTagsFromRoomDirection(roomPath[i], roomPath[i - 1]);
            }
            if(i + 1 < roomPath.Count)
            {
                currentTags |= GetTagsFromRoomDirection(roomPath[i], roomPath[i + 1]);
            }

            CopyRoomToPlace(roomPath[i], roomSize, GetRoom(currentTags, RoomTags.Optional));
        }

        /*for (int i = 0; i < _roomGrid._gridSize.x; i++)
        {
            for(int j = 0; j < _roomGrid._gridSize.y; j++)
            {
                currentTags = RoomTags.None;
                currentTags = i%2 == 0 ? RoomTags.EntranceWest : RoomTags.EntranceNorth;

                CopyRoomToPlace(new Vector2Int(i, j), roomSize, GetRoom(currentTags));
            }
        }*/
    }


    private RoomTemplate GetRoom(RoomTags roomTags, RoomTags excludeTags)
    {
        List<RoomTemplate> possibleRooms = new List<RoomTemplate>();

        for(int i = 0; i < _rooms.Count; i++)
        {
            if (_rooms[i].tags.HasFlag(roomTags) && !_rooms[i].tags.HasFlag(excludeTags))
                possibleRooms.Add(_rooms[i]);
        }

        return possibleRooms[Random.Range(0, possibleRooms.Count)];
    }
    
    private RoomTemplate GetRoomExclusivelyWithTags(RoomTags roomTags)
    {
        List<RoomTemplate> possibleRooms = new List<RoomTemplate>();

        RoomTags otherTags = ~roomTags;

        for(int i = 0; i < _rooms.Count; i++)
        {
            if (_rooms[i].tags.HasFlag(roomTags) && !_rooms[i].tags.HasFlag(otherTags))
                possibleRooms.Add(_rooms[i]);
        }

        return possibleRooms[Random.Range(0, possibleRooms.Count)];
    }

    private RoomTags GetTagsFromRoomDirection(Vector2Int currentRoom, Vector2Int neighbour)
    {
        RoomTags tags = RoomTags.None;

        if(currentRoom.x > neighbour.x)
        {
            tags |= RoomTags.EntranceWest;
        }
        else if(currentRoom.x < neighbour.x)
        {
            tags |= RoomTags.EntranceEast;
        }
        else if(currentRoom.y > neighbour.y) 
        {
            tags |= RoomTags.EntranceSouth;
        }
        else if(neighbour.y < currentRoom.y)
        {
            tags |= RoomTags.EntranceNorth;
        }

        return tags;
    }


    private void CalculatePath(Vector2Int currentRoom, ref List<Vector2Int> roomPath)
    {
        int leftRightDir = 0;

        int dirRNG = Random.Range(1, 6);
        if (dirRNG == 5)
        {
            currentRoom.y -= 1;
            if(currentRoom.y < 0)
            {
                currentRoom.y = 0;
                roomPath.Add(currentRoom);
                return;
            }

            roomPath.Add(currentRoom);
            CalculatePath(currentRoom, ref roomPath);
            return;
        }
        else if (dirRNG == 1 || dirRNG == 2)
        {
            if (currentRoom.x == 0)
                leftRightDir = 1;
            else
                leftRightDir = -1;
        }
        else if (dirRNG == 3 || dirRNG == 4)
        {
            if (currentRoom.x == _roomGrid._gridSize.x - 1)
                leftRightDir = -1;
            else
                leftRightDir = 1;
        }


        do
        {
            currentRoom.x += leftRightDir;
            roomPath.Add(currentRoom);

            dirRNG = Random.Range(1, 6);

        } while (dirRNG != 5 && currentRoom.x > 0 && currentRoom.x < _roomGrid._gridSize.x - 1);

        currentRoom.y -= 1;

        if (currentRoom.y < 0)
        {
            currentRoom.y = 0;
            roomPath.Add(currentRoom);
            return;
        }

        roomPath.Add(currentRoom);
        CalculatePath(currentRoom, ref roomPath);

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
