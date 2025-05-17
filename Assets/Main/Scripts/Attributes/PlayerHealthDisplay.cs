using AMAZON.Attributes;
using AMAZON.Combat;
using AMAZON.Stats;
using TMPro;
using UniRx;
using UnityEngine;

namespace AMAZON.UI
{
    public class PlayerHealthDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _healthText;

        private Health _health;

        private void Start()
        {
            _health = GameObject.FindWithTag("Player").GetComponent<Health>();

            _health.CurrentHealth.Subscribe(newValue =>
            {
                _healthText.SetText(string.Format("Health: {0:0.0}%", _health.GetHealthPercent()));
            })
            .AddTo(this);
        }
    }
}