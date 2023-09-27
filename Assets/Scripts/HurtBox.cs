using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.Events;

public enum CollisionType
{
    box,
    sphere
}

public class HurtBox : MonoBehaviour
{
    [SerializeField] internal Transform _point;
    [SerializeField] private CollisionType collisionType;
    [SerializeField] private int damage;
    [SerializeField] private float knockbackVelocity = 5;
    [SerializeField] private LayerMask _collisionMask;
    [SerializeField] private bool _oneTimeHit = false;
    [SerializeField] private bool _targetOnEnable = false;
    private List<Collider2D> _targets = new List<Collider2D>();

    private Vector2 _pos;

    [SerializeField, HideInInspector]
    internal Vector2 _size;

    private HashSet<Collider2D> _alreadyHit = new HashSet<Collider2D>();

    public UnityAction OnHit = delegate { };

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

        if (_targetOnEnable)
            _targets = GetOverlaps().ToList();

        _alreadyHit.Clear();
    }


    private void FixedUpdate()
    {
        if (_point != null)
        {
            _pos = _point.position;
            _size = _point.localScale;
        }


        if (_targetOnEnable)
        {
            if (_targets != null && _targets.Count > 0)
            {
                OnHit?.Invoke();
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

            return;
        }

        Collider2D[] result = GetOverlaps();
    
        if(result != null && result.Length > 0)
        {
            OnHit?.Invoke();

            for(int i = 0; i < result.Length; i++)
            {
                if (!_alreadyHit.Contains(result[i]) && result[i].TryGetComponent(out IDamageable damageable))
                {
                    if (damageable.CanBeHit())
                    {
                        if(_oneTimeHit)
                            _alreadyHit.Add(result[i]);
                        
                        Vector2 dir = (Vector2)(result[0].transform.position - transform.position).normalized;
                        damageable.TakeDamage(damage, dir * knockbackVelocity);
                    }
                }
            }
        }
    }

    private Collider2D[] GetOverlaps()
    {
        switch (collisionType)
        {
            case CollisionType.box:
                return Physics2D.OverlapBoxAll(_pos, _size, 0, _collisionMask);

            case CollisionType.sphere:
                return Physics2D.OverlapCircleAll(_pos, _size.x / 2, _collisionMask);

            default:
                return null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (!_point)
        {
            switch (collisionType)
            {
                case CollisionType.box:
                    Gizmos.DrawWireCube(transform.position, _size);
                    break;

                case CollisionType.sphere:
                    Gizmos.DrawWireSphere(transform.position, _size.x / 2);
                    break;
            }
            
        }
        else
        {
            switch (collisionType)
            {
                case CollisionType.box:
                    Gizmos.DrawWireCube(_point.position, _point.localScale);
                    break;

                case CollisionType.sphere:
                    Gizmos.DrawWireSphere(_point.position, _point.localScale.x / 2);
                    break;
            }
        }
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