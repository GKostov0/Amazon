using AMAZON.Core;
using AMAZON.Movement;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AMAZON.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private Mover _mover;
        [SerializeField] private ActionScheduler _actionScheduler;
        [SerializeField] private Animator _animator;
        [SerializeField][Range(0.0f, 10.0f)] private float _weaponRange;
        [SerializeField][Range(0.0f, 10.0f)] private float _timeBetweenAttacks;
        [SerializeField] private GameObject _weaponPrefab = null;
        [SerializeField] private Transform _weaponSocket = null;

        [InfoBox("Min Max Weapon Damage")]
        [MinMaxSlider(0, 50, true)]
        [SerializeField] private Vector2 _weaponDamage;

        private Health _target;

        private float _timeSinceLastAttack = Mathf.Infinity;

        private void SpawnWeapon() => Instantiate(_weaponPrefab, _weaponSocket);

        private void Start()
        {
            SpawnWeapon();
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (_target == null) return;
            if (_target.IsDead()) return;

            if (!IsInRange())
            {
                _mover.MoveTo(_target.transform.position, 1.0f);
            }
            else
            {
                AttackBehaviour();
                _mover.Cancel();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(_target.transform);
            
            if (_timeSinceLastAttack >= _timeBetweenAttacks)
            {
                TriggerAttackAnimation();
                _timeSinceLastAttack = 0.0f;
            }
        }

        private void TriggerAttackAnimation()
        {
            _animator.ResetTrigger("stopAttack");

            // This will trigger the Hit() event
            _animator.SetTrigger("attack");
        }

        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, _target.transform.position) < _weaponRange;
        }

        public void Cancel()
        {
            _target = null;
            StopAttackAnimation();
            _mover.Cancel();
        }

        private void StopAttackAnimation()
        {
            _animator.ResetTrigger("attack");
            _animator.SetTrigger("stopAttack");
        }

        public void Attack(GameObject target)
        {
            _actionScheduler.StartAction(this);
            _target = target.GetComponent<Health>();
        }

        public bool CanAttack(GameObject target)
        {
            if (target == null) return false;

            target.TryGetComponent<Health>(out var targetHealth);
            return targetHealth != null && !targetHealth.IsDead();
        }

        // Animation Event
        private void Hit()
        {
            if (_target != null)
            {
                _target.TakeDamege(Random.Range(_weaponDamage.x, _weaponDamage.y));
            }
        }
    }
}