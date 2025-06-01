using AMAZON.Core;
using AMAZON.Movement;
using AMAZON.Saving;
using AMAZON.Attributes;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UniRx;
using AMAZON.Stats;
using System.Collections.Generic;

namespace AMAZON.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] private Mover _mover;
        [SerializeField] private ActionScheduler _actionScheduler;
        [SerializeField] private Animator _animator;
        [SerializeField][Range(0.0f, 10.0f)] private float _timeBetweenAttacks;
        [SerializeField] private Transform _rightHandSocket = null;
        [SerializeField] private Transform _leftHandSocket = null;
        [SerializeField] private BaseStats _baseStats = null;

        [SerializeField] private WeaponSO _defaultWeapon = null;

        private WeaponSO _currentWeapon;

        public ReactiveProperty<Health> Target { get; private set; } = new ReactiveProperty<Health>();

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

            if (Target.Value == null) return;
            if (Target.Value.IsDead()) return;

            if (!IsInRange())
            {
                _mover.MoveTo(Target.Value.transform.position, 1.0f);
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

        public IEnumerable<float> GetAdditiveModifier(EStat stat)
        {
            if (stat.Equals(EStat.Damage))
            {
                yield return Random.Range(_currentWeapon.GetWeaponDamage().x, _currentWeapon.GetWeaponDamage().y);
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(Target.Value.transform);
            
            if (_timeSinceLastAttack >= _timeBetweenAttacks)
            {
                TriggerAttackAnimation();
                _timeSinceLastAttack = 0.0f;
            }
        }

        private void TriggerAttackAnimation()
        {
            _animator.ResetTrigger("stopAttack");

            // This will trigger the Hit() Method
            _animator.SetTrigger("attack");
        }

        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, Target.Value.transform.position) < _currentWeapon.GetWeaponRange();
        }

        public void Cancel()
        {
            Target.Value = null;

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
            Target.Value = target.GetComponent<Health>();
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
            if (Target.Value == null) return;
            
            if (_currentWeapon.HasProjectile())
            {
                _currentWeapon.LaunchProjectile(_rightHandSocket, _leftHandSocket, Target.Value, gameObject, _baseStats.GetStat(EStat.Damage));
            }
            else
            {
                Target.Value.TakeDamege(gameObject, _baseStats.GetStat(EStat.Damage));
            }
        }
    }
}