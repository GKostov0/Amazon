using AMAZON.Core;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField][Range(2.0f, 45.0f)] private float _speed = 9.0f;
    [SerializeField] private Collider _collider;

    private Health _target = null;
    private float _damage = 0;

    private bool FlipCoin() => Random.value < 0.5f;
    private float GetOffset() => Random.Range(0.1f, 1.0f);

    private void Start()
    {
        transform.LookAt(GetAimLocation());
    }

    private void Update()
    {
        if (_target)
        {
            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }
    }

    public void SetTarget(Health target, float damage)
    {
        _target = target;
        _damage = damage;
    }
    private Vector3 GetAimLocation()
    {
        Collider collider = _target.GetComponent<Collider>();
        return collider ? collider.bounds.center + (Vector3.up * (FlipCoin() ? GetOffset() : -GetOffset())) : _target.transform.position;
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
        }

        Destroy(gameObject, 5.0f);
    }
}