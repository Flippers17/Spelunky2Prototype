using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pot : Item
{
    [SerializeField]
    private List<GameObject> possibleSpawns = new List<GameObject>();

    [SerializeField, Range(0, 1)] private float dropRate = 0.5f;

    public override void Die()
    {
        float rng1 = Random.Range(0, 100)/100;
        if (rng1 <= dropRate)
        {
            Instantiate(possibleSpawns[Random.Range(0, possibleSpawns.Count)], transform.position, quaternion.identity);
        }
        
        base.Die();
    }
}
