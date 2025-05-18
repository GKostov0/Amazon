using Sirenix.OdinInspector;
using UnityEngine;

namespace AMAZON.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField][Range(1, 40)] private int _startingLevel = 1;
        [SerializeField] private ECharacterClass _charecterClass;
        [SerializeField] private Experience _experience;

        [AssetsOnly]
        [SerializeField] private ProgressionSO _progression = null;

        public float GetStat(EStat stat) => _progression.GetStat(stat, _charecterClass, GetLevel());

        public int GetLevel()
        {
            if (_experience == null) return _startingLevel;

            float currentXP = _experience.ExperiencePoints.Value;

            int maxLevel = _progression.GetLevels(EStat.ExperienceToLevelUp, _charecterClass);

            for (int level = 1; level <= maxLevel; level++)
            {
                float xpToLevelUp = _progression.GetStat(EStat.ExperienceToLevelUp, _charecterClass, level);

                if (xpToLevelUp > currentXP)
                {
                    return level;
                }
            }

            return maxLevel + 1;
        }
    }
}