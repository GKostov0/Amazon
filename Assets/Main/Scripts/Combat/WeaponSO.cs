using AMAZON.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AMAZON.Combat
{
    [CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Make new weapon", order = 0)]
    public class WeaponSO : ScriptableObject
    {
        [AssetsOnly]
        [SerializeField] private AnimatorOverrideController _animatorOverride = null;

        [AssetsOnly]
        [SerializeField] private GameObject _weaponPrefab = null;

        [AssetsOnly]
        [SerializeField] private Projectile _projectile = null;

        [BoxGroup("WeaponStats")]
        [LabelText("Range")]
        [SerializeField][Range(0.0f, 10.0f)] private float _weaponRange;

        [BoxGroup("WeaponStats")]
        [LabelText("Min Max Damage")]
        [MinMaxSlider(0, 50, true)]
        [SerializeField] private Vector2 _weaponDamage;

        [SerializeField] private bool _isRightHand = true;

        private const string weaponName = "Weapon";

        public Vector2 GetWeaponDamage() => _weaponDamage;
        public float GetWeaponRange() => _weaponRange;

        private Transform GetHandTransform(Transform rHand, Transform lHand) => _isRightHand ? rHand : lHand;

        public bool HasProjectile() => _projectile != null;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            if (_weaponPrefab)
            {
                GameObject weapon = Instantiate(_weaponPrefab, GetHandTransform(rightHand, leftHand));
                weapon.name = weaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (_animatorOverride)
                animator.runtimeAnimatorController = _animatorOverride;
            else if (overrideController != null)
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator)
        {
            Projectile projectile = Instantiate(_projectile, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);
            projectile.SetTarget(target, instigator, Random.Range(_weaponDamage.x, _weaponDamage.y));
        }

        private void DestroyOldWeapon(Transform rHand, Transform lHand)
        {
            Transform oldWeapon = rHand.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = lHand.Find(weaponName);
            }

            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }
    }
}