using UnityEngine;

namespace AMAZON.Stats
{
    [CreateAssetMenu(fileName = "NewCharacterProgression", menuName = "Stats/CharacterProgression")]
    [System.Serializable]
    public class CharacterProgressionSO : ScriptableObject
    {
        [field: SerializeField] public ECharacterClass CharacterClass { get; private set; }
        [field: SerializeField] public float[] HealthPoints { get; private set; }
    }
}