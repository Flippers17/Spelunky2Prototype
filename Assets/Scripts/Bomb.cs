using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Item
{
    [SerializeField]
    private float _fuseTime = 2;
    private float _timer = 0;

    [SerializeField]
    private bool _fused = false;

    [SerializeField]
    private GameObject explosion;

    protected override void OnEnable()
    {
        base.OnEnable();
        _health.OnDie -= Die;
        _health.OnDie += Explode;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _health.OnDie -= Explode;
    }

    protected override void Update()
    {
        base.Update();
        if (_fused)
        {
            if (_timer < _fuseTime)
                _timer += Time.deltaTime;
            else
                TakeDamage(99, Vector2.zero);
        }
    }

    public override void ThrowItem(Vector2 velocity)
    {
        base.ThrowItem(velocity);
        _fused = true;
    }

    public override void PlaceItemDown()
    {
        base.PlaceItemDown();
        _fused = true;
    }

    public void Explode()
    {
        if (explosion)
            Instantiate(explosion, transform.position, Quaternion.identity);

        Die();
    }
}
