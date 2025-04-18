using UnityEngine;

namespace AMAZON.Stats
{
    [CreateAssetMenu(fileName = "NewProgression", menuName = "Stats/Progression")]
    public class ProgressionSO : ScriptableObject
    {
        [SerializeField] private CharacterProgression[] _characterClasses = null;

        [System.Serializable]
        private class CharacterProgression
        {
            [SerializeField] private ECharecterClass _characterClass;
            [SerializeField] private float[] _health;
        }
    }
}