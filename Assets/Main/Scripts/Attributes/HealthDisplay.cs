using AMAZON.Attributes;
using TMPro;
using UnityEngine;

namespace AMAZON.UI
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _playerHealthText;
        [SerializeField] private TextMeshProUGUI _targetHealthText;

        private Health _playerHealth;
        private Health _targetHealth;

        private void Awake()
        {
            _playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            _playerHealthText.SetText(string.Format("Health: {0:0.0}%", _playerHealth.GetHealthPercent()));

            if (_targetHealth)
                _targetHealthText.SetText(string.Format("Target: {0:0.0}%", _targetHealth.GetHealthPercent()));
        }
    }
}