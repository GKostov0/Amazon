using AMAZON.Audio;
using AMAZON.Core;
using AMAZON.Saving;
using AMAZON.Stats;
using DamageNumbersPro;
using Newtonsoft.Json.Linq;
using System.Collections;
using UniRx;
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
        [SerializeField] private DamageNumber _damageNumber;
        [SerializeField] private AudioRandomizer _takeDamageAudio;
        [SerializeField] private AudioRandomizer _dieAudio;

        public ReactiveProperty<float> CurrentHealth { get; private set; } = new ReactiveProperty<float>();
        public ReactiveProperty<float> NormalizedHealth { get; private set; }

        private ReactiveProperty<bool> _onRestoreComplete = new ReactiveProperty<bool>(false);

        private bool _isDead;

        public bool IsDead() => _isDead;
        public Transform GetModel() => _model;
        public JToken CaptureAsJToken() => JToken.FromObject(CurrentHealth.Value);

        // Normalized (0 - 1)
        public float GetHealthFraction() => CurrentHealth.Value / _baseStats.GetStat(EStat.Health);
        public float GetHealthPercent() => 100.0f * GetHealthFraction();
        public float GetMaxHealth() => _baseStats.GetStat(EStat.Health);

        private void Awake()
        {
            NormalizedHealth = new ReactiveProperty<float>(GetHealthFraction());
        }

        private IEnumerator Start()
        {
            if (PlayerPrefs.GetInt("save", 0) == 1)
            {
                while (!_onRestoreComplete.Value)
                {
                    yield return null;
                }
            }

            if (CurrentHealth.Value <= 0)
            {
                CurrentHealth.Value = _baseStats.GetStat(EStat.Health);
                NormalizedHealth.Value = GetHealthFraction();
            }

            _baseStats.CurrentLevel.Subscribe(newValue =>
            {
                CurrentHealth.Value = _baseStats.GetStat(EStat.Health);
                NormalizedHealth.Value = GetHealthFraction();
            })
            .AddTo(this);
        }

        public void RestoreFromJToken(JToken state)
        {
            CurrentHealth.Value = state.ToObject<float>();
            NormalizedHealth.Value = GetHealthFraction();
            _onRestoreComplete.Value = true;
        }

        public void TakeDamege(GameObject instigator, float amount)
        {
            // Debug.Log($"{gameObject.name} took {amount} damage!");
            _takeDamageAudio.PlaySound();

            CurrentHealth.Value = Mathf.Max(CurrentHealth.Value - amount, 0.0f);
            NormalizedHealth.Value = GetHealthFraction();

            _damageNumber.Spawn(transform.position + Vector3.up * 2.0f, amount);

            if (CurrentHealth.Value <= 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public void Heal(float pointsToRestore)
        {
            CurrentHealth.Value = Mathf.Min(CurrentHealth.Value + pointsToRestore, GetMaxHealth());
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
            _dieAudio.PlaySound();
            _animator.SetTrigger("die");
            _actionScheduler.CancelCurrentAction();
        }
    }
}