using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace AMAZON.Stats
{
    [CreateAssetMenu(fileName = "NewProgression", menuName = "Stats/Progression")]
    public class ProgressionSO : ScriptableObject
    {
        [AssetsOnly]
        [InlineEditor(InlineEditorModes.GUIAndHeader)]
        [SerializeField] private CharacterProgressionSO[] _characterClasses = null;

        private Dictionary<ECharacterClass, Dictionary<EStat, float[]>> _lookupTable = null;

        public float GetStat(EStat stat, ECharacterClass characterClass, int level)
        {
            BuildLookupTable();

            float[] levels = _lookupTable[characterClass][stat];

            return (levels.Length < level) ? 0.0f : levels[level - 1];
        }

        private void BuildLookupTable()
        {
            if (_lookupTable != null) return;

            _lookupTable = new Dictionary<ECharacterClass, Dictionary<EStat, float[]>>();

            _characterClasses.ForEach(character => 
            {
                var statLookupTable = new Dictionary<EStat, float[]>();

                character.stats.ForEach(x => statLookupTable[x.stat] = x.levels);

                _lookupTable[character.CharacterClass] = statLookupTable;
            });
        }
    }
}