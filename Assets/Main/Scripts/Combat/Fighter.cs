using AMAZON.Core;
using AMAZON.Movement;
using AMAZON.Saving;
using AMAZON.Attributes;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace AMAZON.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private Mover _mover;
        [SerializeField] private ActionScheduler _actionScheduler;
        [SerializeField] private Animator _animator;
        [SerializeField][Range(0.0f, 10.0f)] private float _timeBetweenAttacks;
        [SerializeField] private Transform _rightHandSocket = null;
        [SerializeField] private Transform _leftHandSocket = null;

        [SerializeField] private WeaponSO _defaultWeapon = null;

        private WeaponSO _currentWeapon;

        private Health _target;

        private float _timeSinceLastAttack = Mathf.Infinity;

        public JToken CaptureAsJToken() => _currentWeapon.name;
        public void RestoreFromJToken(JToken state) => EquipWeapon(Resources.Load<WeaponSO>((string)state));

        private void Start()
        {
            if (_currentWeapon == null)
                EquipWeapon(_defaultWeapon);
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

        public void EquipWeapon(WeaponSO weapon)
        {
            _currentWeapon = weapon;
            weapon.Spawn(_rightHandSocket, _leftHandSocket, _animator);
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
            return Vector3.Distance(transform.position, _target.transform.position) < _currentWeapon.GetWeaponRange();
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
            if (_target == null) return;
            
            if (_currentWeapon.HasProjectile())
            {
                _currentWeapon.LaunchProjectile(_rightHandSocket, _leftHandSocket, _target);
            }
            else
            {
                _target.TakeDamege(Random.Range(_currentWeapon.GetWeaponDamage().x, _currentWeapon.GetWeaponDamage().y));
            }
        }
    }
}