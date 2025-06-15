using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UniRx;
using System;
using System.Collections;

namespace AMAZON.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField][Range(1, 40)] private int _startingLevel = 1;
        [SerializeField] private ECharacterClass _charecterClass;
        [SerializeField] private Experience _experience;
        [SerializeField] private ParticleSystem _levelUpPartilce;
        [SerializeField] private bool _useModifiers = false;

        [AssetsOnly]
        [SerializeField] private ProgressionSO _progression = null;

        public ReactiveProperty<int> CurrentLevel { get; private set; } = new ReactiveProperty<int>(1);

        private float GetBaseStat(EStat stat) => _progression.GetStat(stat, _charecterClass, CurrentLevel.Value);
        public float GetStat(EStat stat) => (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100.0f);

        private IEnumerator Start()
        {
            if (_experience)
            {
                if (PlayerPrefs.GetInt("save", 0) == 1)
                {
                    while (!_experience.OnRestoreComplete.Value)
                    {
                        yield return null;
                    }
                }

                CurrentLevel.Value = CalculateLevel();

                _experience.ExperiencePoints.Subscribe(_ =>
                {
                    UpdateLevel();
                })
                .AddTo(this);
            }
        }

        private float GetPercentageModifier(EStat stat)
        {
            if (!_useModifiers) return 0;

            float totalResult = 0;
            GetComponents<IModifierProvider>().ForEach(provider =>
            {
                provider.GetPercentageModifiers(stat).ForEach(modifier =>
                {
                    totalResult += modifier;
                });
            });

            return totalResult;
        }

        private float GetAdditiveModifier(EStat stat)
        {
            if (!_useModifiers) return 0;

            float totalResult = 0;
            GetComponents<IModifierProvider>().ForEach(provider => 
            {
                provider.GetAdditiveModifiers(stat).ForEach(modifier => 
                {
                    totalResult += modifier;
                });
            });

            return totalResult;
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();

            if (newLevel > CurrentLevel.Value)
            {
                CurrentLevel.Value = newLevel;
                Debug.Log($"{name} Level UP!");

                if (_levelUpPartilce)
                {
                    _levelUpPartilce.Play(true);
                }
            }
        }

        private int CalculateLevel()
        {
            if (_experience == null) return _startingLevel;

            int maxLevel = _progression.GetLevels(EStat.ExperienceToLevelUp, _charecterClass);

            for (int level = 1; level <= maxLevel; level++)
            {
                float xpToLevelUp = _progression.GetStat(EStat.ExperienceToLevelUp, _charecterClass, level);

                if (xpToLevelUp > _experience.ExperiencePoints.Value)
                {
                    return level;
                }
            }

            return maxLevel + 1;
        }
    }
}