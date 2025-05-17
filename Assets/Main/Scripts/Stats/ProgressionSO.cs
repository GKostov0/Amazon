using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

namespace AMAZON.Stats
{
    [CreateAssetMenu(fileName = "NewProgression", menuName = "Stats/Progression")]
    public class ProgressionSO : ScriptableObject
    {
        [AssetsOnly]
        [InlineEditor(InlineEditorModes.GUIAndHeader)]
        [SerializeField] private CharacterProgressionSO[] _characterClasses = null;

        public float GetStat(EStat stat, ECharacterClass characterClass, int level)
        {
            CharacterProgressionSO characterContext = _characterClasses.FirstOrDefault(x => x.CharacterClass.Equals(characterClass));
            ProgressionStat statContext = characterContext.stats.FirstOrDefault(x => x.stat.Equals(stat));

            return statContext.levels[level - 1];
        }
    }
}