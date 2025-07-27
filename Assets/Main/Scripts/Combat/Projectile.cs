using AMAZON.Attributes;
using AMAZON.Audio;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AMAZON.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private GameObject _trailEffect;
        [SerializeField] private GameObject _explosionEffect;
        [SerializeField][Range(2.0f, 45.0f)] private float _speed = 9.0f;
        [SerializeField] private Collider _collider;
        [SerializeField] private bool _isHoaming;
        [SerializeField] private bool _destroyTrailOnImpact;
        [SuffixLabel("s", Overlay = true)][SerializeField] private float _destroyAfter;
        [SerializeField] private AudioRandomizer _audioRandomizerLaunch;
        [SerializeField] private AudioRandomizer _audioRandomizerHit;

        private Health _target = null;
        private float _damage = 0;
        private float _targetOffset;
        private Vector3 _targetPosition;
        private GameObject _instigator = null;

        private bool FlipCoin() => Random.value < 0.5f;
        private float GetOffset() => Random.Range(0.3f, 0.7f);
        private float GetRandomOffset() => FlipCoin() ? GetOffset() : -GetOffset();

        private void Start()
        {
            _audioRandomizerLaunch.PlaySound();
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

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            _target = target;
            _damage = damage;
            _instigator = instigator;
        }

        private Vector3 GetAimLocation()
        {
            Collider collider = _target.GetComponent<Collider>();
            return collider ? collider.bounds.center + _targetPosition : _target.transform.position;
        }

        private void HandlePArticleEffects()
        {
            if (_destroyTrailOnImpact && _trailEffect)
                Destroy(_trailEffect, 0.5f);

            if (_explosionEffect)
                _explosionEffect.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            Health health = other.GetComponent<Health>();
            if (health != _target) return;

            if (other.CompareTag("Enemy") || other.CompareTag("Player"))
            {
                HandlePArticleEffects();

                _target.TakeDamege(_instigator, _damage);
                transform.parent = health.GetModel();
                _audioRandomizerHit.PlaySound();

                _speed = 0.0f;
                _target = null;
                _collider.enabled = false;

                if (health.IsDead())
                    other.enabled = false;
            }

            Destroy(gameObject, _destroyAfter);
        }
    }
}