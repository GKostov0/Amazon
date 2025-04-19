using Sirenix.OdinInspector;
using UnityEngine;

namespace AMAZON.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField][Range(1, 40)] private int _startingLevel = 1;
        [SerializeField] private ECharacterClass _charecterClass;

        [AssetsOnly]
        [SerializeField] private ProgressionSO _progression = null;

        public float GetHealth() => _progression.GetHealth(_charecterClass, _startingLevel);
    }
}