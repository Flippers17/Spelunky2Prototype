using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class DestructableTilemap : MonoBehaviour
{
    public Tilemap tilemap;


    private void OnValidate()
    {
        if (tilemap == null)
            if (!TryGetComponent(out tilemap))
                Debug.LogWarning("DestructableTilemap is missing Tilemap reference", this);
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
            //Debug.Log(collision.gameObject.name);
        if(collider.gameObject.CompareTag("Explosion"))
        {
            CircleCollider2D circle = (CircleCollider2D)collider;
            float radius = circle.radius;


            foreach(Vector3Int tile in GetTilesWithinRadius(collider.transform.position, radius))
            {
                if(tile != null)
                {
                    tilemap.SetTile(tile, null);
                }
            }
        }
    }


    private List<Vector3Int> GetTilesWithinRadius(Vector2 pos, float radius)
    {
        List<Vector3Int> results = new List<Vector3Int>();

        float xPos = pos.x - radius;
        float yPos = pos.y - radius;

        for(int x = -Mathf.FloorToInt(radius); x <= Mathf.FloorToInt(radius); x++)
        {
            for(int y = -Mathf.FloorToInt(radius); y <= Mathf.FloorToInt(radius); y++)
            {
                if(((x * x) + (y * y)) < (radius * radius))
                {
                    results.Add(tilemap.WorldToCell(pos + new Vector2(x, y)));
                }
            }
        }

        return results;
    }
}
