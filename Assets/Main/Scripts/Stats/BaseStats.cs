using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UniRx;

namespace AMAZON.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField][Range(1, 40)] private int _startingLevel = 1;
        [SerializeField] private ECharacterClass _charecterClass;
        [SerializeField] private Experience _experience;
        [SerializeField] private ParticleSystem _levelUpPartilce;

        [AssetsOnly]
        [SerializeField] private ProgressionSO _progression = null;

        public ReactiveProperty<int> CurrentLevel { get; private set; } = new ReactiveProperty<int>();

        public float GetStat(EStat stat) => _progression.GetStat(stat, _charecterClass, CurrentLevel.Value) + GetAdditiveModifier(stat);

        private void Awake()
        {
            CurrentLevel.Value = CalculateLevel();

            if (_experience)
            {
                _experience.ExperiencePoints.Subscribe(_ =>
                {
                    UpdateLevel();
                })
                .AddTo(this);
            }
        }

        private float GetAdditiveModifier(EStat stat)
        {
            float totalResult = 0;

            GetComponents<IModifierProvider>().ForEach(provider => 
            {
                provider.GetAdditiveModifier(stat).ForEach(modifier => 
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