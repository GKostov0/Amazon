using AMAZON.Stats;
using TMPro;
using UnityEngine;
using UniRx;

namespace AMAZON.UI
{
    public class PlayerLevelDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelText;

        private BaseStats _baseStats;

        private void Start()
        {
            _baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();

            _baseStats.CurrentLevel.Subscribe(newLevel =>
            {
                _levelText.SetText($"Level: {newLevel}");
            })
            .AddTo(this);
        }
    }
}