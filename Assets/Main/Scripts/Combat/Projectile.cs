using AMAZON.Core;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField][Range(2.0f, 45.0f)] private float _speed = 9.0f;
    [SerializeField] private Collider _collider;
    [SerializeField] private bool _isHoaming;

    private Health _target = null;
    private float _damage = 0;
    private float _targetOffset;
    private Vector3 _targetPosition;

    private bool FlipCoin() => Random.value < 0.5f;
    private float GetOffset() => Random.Range(0.1f, 1.0f);
    private float GetRandomOffset() => FlipCoin() ? GetOffset() : -GetOffset();

    private void Start()
    {
        _targetOffset = GetRandomOffset();
        _targetPosition = Vector3.up * _targetOffset;

        transform.LookAt(GetAimLocation());
        Destroy(gameObject, 10.0f);
    }

    private void Update()
    {
        if (_target == null) return;

        if (_isHoaming && !_target.IsDead())       
            transform.LookAt(GetAimLocation());

        transform.Translate(_speed * Time.deltaTime * Vector3.forward);
    }

    public void SetTarget(Health target, float damage)
    {
        _target = target;
        _damage = damage;
    }

    private Vector3 GetAimLocation()
    {
        Collider collider = _target.GetComponent<Collider>();
        return collider ? collider.bounds.center + _targetPosition : _target.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();
        if (health != _target) return;

        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            _target.TakeDamege(_damage);
            transform.parent = health.GetModel();
            _target = null;
            _collider.enabled = false;
            if (health.IsDead())
            {
                other.enabled = false;
            }
        }

        Destroy(gameObject, 5.0f);
    }
}