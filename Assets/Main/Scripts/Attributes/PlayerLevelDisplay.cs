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
        private Experience _playerExperience;

        private void Start()
        {
            _baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            _playerExperience = GameObject.FindWithTag("Player").GetComponent<Experience>();

            _playerExperience.ExperiencePoints.Subscribe(_ =>
            {
                _levelText.SetText($"Level: {_baseStats.CurrentLevel.Value}");
            })
            .AddTo(this);
        }
    }
}