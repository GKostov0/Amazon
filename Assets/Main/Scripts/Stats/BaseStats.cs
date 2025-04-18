using Sirenix.OdinInspector;
using UnityEngine;

namespace AMAZON.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField][Range(1, 40)] private int _startingLevel = 1;
        [SerializeField] private ECharecterClass _charecterClass;

        [AssetsOnly]
        [SerializeField] private ProgressionSO _progression = null;
    }
}