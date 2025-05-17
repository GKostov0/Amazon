using AMAZON.Core;
using AMAZON.Saving;
using AMAZON.Stats;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
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
        [ReadOnly][SerializeField] private float _health = 100.0f;

        private bool _isDead;

        public bool IsDead() => _isDead;
        public Transform GetModel() => _model;
        public JToken CaptureAsJToken() => JToken.FromObject(_health);
        public float GetHealthPercent() => 100.0f * (_health / _baseStats.GetStat(EStat.Health));

        private void Start()
        {
            // TODO: reset health from stats or restore from save?
            _health = _baseStats.GetStat(EStat.Health);
        }

        public void RestoreFromJToken(JToken state)
        {
            // TODO: reset health from stats or restore from save?
            _health = state.ToObject<float>();
            if (_health <= 0) Die();
        }

        public void TakeDamege(GameObject instigator, float amount)
        {
            _health = Mathf.Max(_health - amount, 0.0f);

            if (_health <= 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience xp = instigator.GetComponent<Experience>();

            if (xp)
            {
                xp.GainExperience(GetComponent<BaseStats>().GetStat(EStat.ExperienceReward));
            }
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