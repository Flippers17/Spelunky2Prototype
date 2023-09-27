using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pot : Item
{
    [SerializeField]
    private List<GameObject> possibleSpawns = new List<GameObject>();

    public override void Die()
    {
        int rng1 = Random.Range(0, 10);
        if (rng1 > 4)
        {
            Instantiate(possibleSpawns[Random.Range(0, possibleSpawns.Count)], transform.position, quaternion.identity);
        }
        
        base.Die();
    }
}
