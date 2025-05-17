using AMAZON.Attributes;
using AMAZON.Combat;
using TMPro;
using UnityEngine;

namespace AMAZON.UI
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _playerHealthText;
        [SerializeField] private TextMeshProUGUI _targetHealthText;

        private Health _playerHealth;
        private Fighter _playerFighter;

        private void Start()
        {
            _playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
            _playerFighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            _playerHealthText.SetText(string.Format("Health: {0:0.0}%", _playerHealth.GetHealthPercent()));

            if (_playerFighter.Target)
                _targetHealthText.SetText(string.Format("Target: {0:0.0}%", _playerFighter.Target.GetHealthPercent()));
            else
                _targetHealthText.SetText("[-]");
        }
    }
}