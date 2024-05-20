using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProbabilisticTile : RuleTile
{
    [Space(15), SerializeField, Range(0f, 1f)]
    private float _probability = 1f;

    [SerializeField]
    private RuleTile tileToPlace;
    //private RuleTile _instance;

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
        //tilemap.GetComponent<Tilemap>().SetTileFlags(position, TileFlags.None);
        //tilemap.GetComponent<Tilemap>().SetColor(position, Color.red);
    }


    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        if (Random.Range(0f, 1f) > _probability && !(Application.isEditor && !Application.isPlaying))
        {
            return;
        }

        
        
        //tileData.color = Color.red;
        base.GetTileData(position, tilemap, ref tileData);
        
        //tilemap.GetComponent<Tilemap>().SetTile(position, null);
        //tilemap.GetComponent<Tilemap>().SetTile(position, this);
    }

    public RuleTile GetTile()
    {
        if (Random.Range(0f, 1f) > _probability)
            return null;

        return tileToPlace;
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
