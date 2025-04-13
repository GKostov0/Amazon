using AMAZON.Saving;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace AMAZON.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private ActionScheduler _actionScheduler;
        [SerializeField] private Transform _model;
        [SerializeField][Range(0.0f, 1000.0f)] private float _health = 100.0f;

        private bool _isDead;

        public bool IsDead() => _isDead;
        public Transform GetModel() => _model;

        public JToken CaptureAsJToken() => JToken.FromObject(_health);
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