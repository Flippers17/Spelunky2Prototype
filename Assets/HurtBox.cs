using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class HurtBox : MonoBehaviour
{
    [SerializeField] internal Transform _point;
    [SerializeField] private int damage;
    [SerializeField] private float knockbackVelocity = 5;
    [SerializeField] private LayerMask _collisionMask;
    [SerializeField] private bool _oneTimeHit = false;

    private Vector2 _pos;

    internal Vector2 _size;

    private List<Collider2D> _targets;

    private void Start()
    {
        if (_point == null)
            _pos = transform.position;
    }

    private void OnEnable()
    {
        if (_point != null)
        {
            _pos = _point.position;
            _size = _point.localScale;
        }

        if ( _oneTimeHit)
            _targets = Physics2D.OverlapBoxAll(_pos, _size, 0, _collisionMask).ToList();
    }


    private void FixedUpdate()
    {
        if (_point != null)
        {
            _pos = _point.position;
            _size = _point.localScale;
        }


        if (_oneTimeHit)
        {

            if (_targets != null && _targets.Count > 0)
            {
                if (_targets[0].TryGetComponent(out IDamageable damageable))
                {
                    if (damageable.CanBeHit())
                    {
                        Vector2 dir = (Vector2)(_targets[0].transform.position - transform.position).normalized;
                        _targets.RemoveAt(0);
                        damageable.TakeDamage(damage, dir * knockbackVelocity);
                    }
                }
            }
        }
        else
        {
            Collider2D[] result = Physics2D.OverlapBoxAll(_pos, _size, 0, _collisionMask);
        
            if(result != null && result.Length > 0)
            {
                if(result[0].TryGetComponent(out IDamageable damageable))
                {
                    if (damageable.CanBeHit())
                    {
                        Vector2 dir = (Vector2)(result[0].transform.position - transform.position).normalized;
                        damageable.TakeDamage(damage, dir * knockbackVelocity);
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (!_point)
            Gizmos.DrawWireCube(transform.position, _size);
        else
            Gizmos.DrawWireCube(_point.position, _point.localScale);
    }
}


[CustomEditor(typeof(HurtBox))]
public class HurtBoxEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var hurtBox = (HurtBox)target;

        if(hurtBox._point == null)
        {
            EditorGUI.BeginChangeCheck();
            Vector2 newSize = EditorGUILayout.Vector2Field("Size", hurtBox._size, new GUILayoutOption[] { });

            if (EditorGUI.EndChangeCheck())
            {
                hurtBox._size = newSize;

                EditorUtility.SetDirty(hurtBox);
            }
        }
    }
}