using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObstacleTile : RuleTile
{

    [SerializeField]
    private List<RoomTemplate> templates = new List<RoomTemplate>();

    public RoomTemplate GetTemplate()
    {
        return templates[Random.Range(0, templates.Count)];
    }


#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a MyTile Asset
    [MenuItem("Assets/Create/Obstacle Tile")]
    public static void CreateMyTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save My Tile", "New My Tile", "Asset", "Save My Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ObstacleTile>(), path);
    }
#endif
}
