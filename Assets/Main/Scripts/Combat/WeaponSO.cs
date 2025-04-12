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

        [BoxGroup("WeaponStats")]
        [LabelText("Range")]
        [SerializeField][Range(0.0f, 10.0f)] private float _weaponRange;

        [BoxGroup("WeaponStats")]
        [LabelText("Min Max Damage")]
        [MinMaxSlider(0, 50, true)]
        [SerializeField] private Vector2 _weaponDamage;

        public Vector2 GetWeaponDamage() => _weaponDamage;
        public float GetWeaponRange() => _weaponRange;

        public void Spawn(Transform handTransform, Animator animator)
        {
            if (_weaponPrefab)
                Instantiate(_weaponPrefab, handTransform);

            if (_animatorOverride)
                animator.runtimeAnimatorController = _animatorOverride;
        }
    }
}