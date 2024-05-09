using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProbabilisticTile : Tile
{
    [Space(15), SerializeField, Range(0f, 1f)]
    private float _probability = 1f;

    [SerializeField]
    private RuleTile _tileToPlace;

    [SerializeField]
    private GameObject _objectToPlace;


    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {

        if (Application.isEditor && !Application.isPlaying)
        {
            base.GetTileData(position, tilemap, ref tileData);
            return;
        }
        if (Random.Range(0f, 1f) < _probability )
            _tileToPlace.GetTileData(position, tilemap, ref tileData);

    }



#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a MyTile Asset
    [MenuItem("Assets/Create/Probabilistic Tile")]
    public static void CreateMyTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save My Tile", "New My Tile", "Asset", "Save My Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ProbabilisticTile>(), path);
    }
#endif
}
