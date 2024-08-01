using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
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

    [SerializeField]
    private List<RuleTile> _treassure = new List<RuleTile>();

    [SerializeField]
    private TileBase _edgeTile;
    [SerializeField]
    private int _edgeThickness = 5;

    [SerializeField]
    private bool _onlyNescessaryDrops;
    
    [SerializeField]
    private bool _optionalRoomsNeedsOptionalTag;


    public static UnityAction OnLevelGenerated;

    public static Vector2Int PlayerStartPos
    {
        get; private set;
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateLevel();
    }


    public void GenerateLevel()
    {
        Vector2Int roomSize = _roomGrid._roomSize;

        Vector2Int startRoom = new Vector2Int(Random.Range(0, _roomGrid._gridSize.x), _roomGrid._gridSize.y - 1);
        RoomTags currentTags = RoomTags.None;
        RoomTags excludingTags = RoomTags.None;

        List<Vector2Int> roomPath = new List<Vector2Int>();
        roomPath.Add(startRoom);
        CalculatePath(startRoom, ref roomPath);
        
        HashSet<Vector2Int> alreadyPlacedRooms = new HashSet<Vector2Int>();

        for(int i = 0; i < roomPath.Count; i++)
        {
            currentTags = RoomTags.None;
            excludingTags = RoomTags.Optional;

            if(i - 1 >= 0)
            {
                currentTags |= GetTagsFromRoomDirection(roomPath[i], roomPath[i - 1]);
            }
            if(i + 1 < roomPath.Count)
            {
                currentTags |= GetTagsFromRoomDirection(roomPath[i], roomPath[i + 1]);
                excludingTags |= RoomTags.MainExit;
                Debug.DrawLine(_roomGrid.bottomLeftCorner + (Vector2)roomPath[i] * roomSize + (roomSize/2), _roomGrid.bottomLeftCorner + (Vector2)roomPath[i + 1] * roomSize + (roomSize / 2), Color.red, 100);
            }
            else
            {
                currentTags |= RoomTags.MainExit;
            }

            if (_onlyNescessaryDrops)
            {
                if(!currentTags.HasFlag(RoomTags.EntranceSouth))
                    excludingTags |= RoomTags.EntranceSouth;
                //Debug.Log(roomPath[i] + " excludes: " + excludingTags);
            }

            CopyRoomToPlace(roomPath[i], roomSize, GetRoom(currentTags, excludingTags), true);
            alreadyPlacedRooms.Add(roomPath[i]);
        }

        Vector2Int currentRoom = Vector2Int.zero;
        for(int i = 0; i < _roomGrid._gridSize.x; i++)
        {
            for(int j = 0; j < _roomGrid._gridSize.y; j++)
            {
                currentRoom = new Vector2Int(i, j);
                if(!alreadyPlacedRooms.Contains(new Vector2Int(i, j)))
                {
                    if(!_optionalRoomsNeedsOptionalTag)
                        CopyRoomToPlace(currentRoom, roomSize, GetRoom(RoomTags.None, RoomTags.MainExit), true);
                    else
                        CopyRoomToPlace(currentRoom, roomSize, GetRoom(RoomTags.Optional, RoomTags.MainExit), true);
                }
            }
        }

        Vector2Int currentLeftCorner = _roomGrid.bottomLeftCorner - new Vector2Int(_edgeThickness, _edgeThickness);
        FillAreaWithTiles(_indestructibleTiles, _edgeTile, currentLeftCorner, currentLeftCorner + new Vector2Int(_edgeThickness - 1, _roomGrid._gridSize.y * roomSize.y + 2 * _edgeThickness - 1));
        currentLeftCorner = _roomGrid.bottomLeftCorner + new Vector2Int(_roomGrid._gridSize.x * roomSize.x - 1, -_edgeThickness);
        FillAreaWithTiles(_indestructibleTiles, _edgeTile, currentLeftCorner, currentLeftCorner + new Vector2Int(_edgeThickness - 1, _roomGrid._gridSize.y * roomSize.y + 2 * _edgeThickness - 1));
        currentLeftCorner = _roomGrid.bottomLeftCorner + new Vector2Int(0, _roomGrid._gridSize.y * roomSize.y);
        FillAreaWithTiles(_indestructibleTiles, _edgeTile, currentLeftCorner, currentLeftCorner + new Vector2Int(_roomGrid._gridSize.x * roomSize.x - 1, _edgeThickness - 1));
        currentLeftCorner = _roomGrid.bottomLeftCorner - new Vector2Int(0, _edgeThickness);
        FillAreaWithTiles(_indestructibleTiles, _edgeTile, currentLeftCorner, currentLeftCorner + new Vector2Int(_roomGrid._gridSize.x * roomSize.x - 1, _edgeThickness - 1));

        PlayerStartPos = FindPlayerStartPos(startRoom, roomSize);
        GenerateTreassure(roomSize);

        Debug.Log(PlayerStartPos);
        OnLevelGenerated?.Invoke();
    }

    public Vector2Int FindPlayerStartPos(Vector2Int startRoomPos, Vector2Int roomSize)
    {
        Vector2Int startCorner = _roomGrid.bottomLeftCorner + new Vector2Int(roomSize.x * startRoomPos.x, roomSize.y * startRoomPos.y);
        Vector2Int current = startCorner;

        for(int x = 0; x < roomSize.x; x++)
        {
            for(int y = 0; y < roomSize.y; y++)
            {
                current = startCorner + new Vector2Int(x, y);
                
                if(!_groundTiles.HasTile((Vector3Int)current) && !_indestructibleTiles.HasTile((Vector3Int)current))
                    return current;
            }
        }

        return startCorner;
    }


    private RoomTemplate GetRoom(RoomTags roomTags, RoomTags excludeTags)
    {
        List<RoomTemplate> possibleRooms = new List<RoomTemplate>();

        for(int i = 0; i < _rooms.Count; i++)
        {
            if (_rooms[i].tags.HasFlag(roomTags) && (_rooms[i].tags & excludeTags) == 0)
                possibleRooms.Add(_rooms[i]);
        }

        return possibleRooms[Random.Range(0, possibleRooms.Count)];
    }
    
    
    private void FillAreaWithTiles(Tilemap tilemap, TileBase tile, Vector2Int bottomLeft, Vector2Int topRight)
    {
        for(int x = bottomLeft.x; x <= topRight.x; x++)
        {
            for(int y = bottomLeft.y; y <= topRight.y; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y), tile);
            }
        }
    }

    private void GenerateTreassure(Vector2Int roomSize)
    {
        if (_treassure.Count == 0)
            return;

        for (int x = _roomGrid.bottomLeftCorner.x; x < _roomGrid.bottomLeftCorner.x + _roomGrid._gridSize.x * roomSize.x; x++)
        {
            for (int y = _roomGrid.bottomLeftCorner.y; y < _roomGrid.bottomLeftCorner.y + _roomGrid._gridSize.y * roomSize.y; y++)
            {
                Vector3Int currentPos = new Vector3Int(x, y);
                if (!_groundTiles.HasTile(currentPos) && !_indestructibleTiles.HasTile(currentPos))
                {
                    if (Random.Range(0, 100) < 5)
                    {
                        _indestructibleTiles.SetTile(currentPos, _treassure[0]);
                    }
                }
            }
        }
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
        else if(currentRoom.y < neighbour.y)
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


    private void CopyRoomToPlace(Vector2Int roomCoordinates, Vector2Int roomSize, RoomTemplate template, bool useGridCoordinates)
    {
        TileBase currentTile;
        List<(ObstacleTile, Vector2Int)> obstacleTiles = new List<(ObstacleTile, Vector2Int)>();

        for(int x = 0; x < roomSize.x; x++)
        {
            for(int y = 0; y < roomSize.y; y++)
            {
                Vector3Int currentPos = new Vector3Int(x, y);
                currentTile = template.groundTiles.GetTile(currentPos + (Vector3Int)template.bottomLeftCorner);
                if (currentTile is ProbabilisticTile)
                {
                    ProbabilisticTile pt = (ProbabilisticTile)currentTile;
                    currentTile = pt.GetTile();

                    if(useGridCoordinates)
                        _groundTiles.SetTile(currentPos + (Vector3Int)_roomGrid.GetRoomCorner(roomCoordinates), currentTile);
                    else
                        _groundTiles.SetTile(currentPos + (Vector3Int)roomCoordinates, currentTile);
                }
                else if(currentTile is ObstacleTile)
                {
                    ObstacleTile ot = (ObstacleTile)currentTile;
                    obstacleTiles.Add((ot, (Vector2Int)currentPos + _roomGrid.GetRoomCorner(roomCoordinates)));
                    
                    //CopyObstacleTemplateToPlace(currentPos, ot.GetTemplate());
                }
                else
                {
                    if (useGridCoordinates)
                        _groundTiles.SetTile(currentPos + (Vector3Int)_roomGrid.GetRoomCorner(roomCoordinates), currentTile);
                    else
                        _groundTiles.SetTile(currentPos + (Vector3Int)roomCoordinates, currentTile);
                }
                    


                currentTile = template.indestructibleTiles.GetTile(currentPos + (Vector3Int)template.bottomLeftCorner);
                if (currentTile is ProbabilisticTile)
                {
                    ProbabilisticTile pt = (ProbabilisticTile)currentTile;
                    currentTile = pt.GetTile();
                }
                if (useGridCoordinates)
                    _indestructibleTiles.SetTile(currentPos + (Vector3Int)_roomGrid.GetRoomCorner(roomCoordinates), currentTile);
                else
                    _indestructibleTiles.SetTile(currentPos + (Vector3Int)roomCoordinates, currentTile);

            }
        }

        for(int i = 0; i < obstacleTiles.Count; i++)
        {
            CopyRoomToPlace(obstacleTiles[i].Item2, new Vector2Int(5, 3), obstacleTiles[i].Item1.GetTemplate(), false);
        }
    }

}
