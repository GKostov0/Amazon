using AMAZON.Core;
using AMAZON.Saving;
using AMAZON.Stats;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace AMAZON.Attributes
{
    [RequireComponent(typeof(BaseStats))]
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private ActionScheduler _actionScheduler;
        [SerializeField] private Transform _model;
        [SerializeField] private BaseStats _baseStats;
        [SerializeField][Range(0.0f, 1000.0f)] private float _health = 100.0f;

        private bool _isDead;

        public bool IsDead() => _isDead;
        public Transform GetModel() => _model;
        public JToken CaptureAsJToken() => JToken.FromObject(_health);
        public float GetHealthPercent() => 100.0f * (_health / _baseStats.GetHealth());

        private void Start()
        {
            _health = _baseStats.GetHealth();
        }

        public void RestoreFromJToken(JToken state)
        {
            _health = state.ToObject<float>();
            if (_health <= 0) Die();
        }

        public void TakeDamege(float amount)
        {
            _health = Mathf.Max(_health - amount, 0.0f);
            print($"[{name}]-Health: " + _health);

            if (_health <= 0) Die();
        }

        private void Die()
        {
            if (_isDead) return;

            _isDead = true;
            _animator.SetTrigger("die");
            _actionScheduler.CancelCurrentAction();
        }
    }
}