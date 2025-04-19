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

        public float GetHealth(ECharacterClass characterClass, int level)
        {
            return _characterClasses.FirstOrDefault(x => x.CharacterClass.Equals(characterClass)).HealthPoints[level - 1];
        }
    }
}