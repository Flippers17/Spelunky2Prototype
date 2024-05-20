using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pot : Item
{
    [SerializeField]
    private List<DropItem> possibleSpawns = new List<DropItem>();

    [SerializeField, Range(0, 1)] private float dropRate = 0.5f;

    public override void Die()
    {
        float rng1 = Random.Range(0, 100)/100;
        if (rng1 <= dropRate)
        {
            float totalWeight = 0;
            for(int i = 0; i < possibleSpawns.Count; i++)
            {
                totalWeight += possibleSpawns[i].dropWeight;
            }

            float dropRNG = Random.Range(0, totalWeight);

            for(int i = 0;i < possibleSpawns.Count; i++)
            {
                if(dropRNG < possibleSpawns[i].dropWeight)
                {
                    Instantiate(possibleSpawns[i].objectToDrop, transform.position + Vector3.up * .1f, quaternion.identity);
                    break;
                }

                dropRNG -= possibleSpawns[i].dropWeight;
            }

            
        }
        
        base.Die();
    }
}

[System.Serializable]
public class DropItem
{
    public GameObject objectToDrop;

    [Range(0, 100)]
    public float dropWeight;
}
