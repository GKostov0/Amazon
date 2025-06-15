using AMAZON.Attributes;
using System.Collections;
using TMPro;
using UniRx;
using UnityEngine;

namespace AMAZON.UI
{
    public class PlayerHealthDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _healthText;

        private Health _health;

        private void Awake()
        {
            _health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(3.0f);

            _health.CurrentHealth.Subscribe(newValue =>
            {
                if (newValue <= -1) return;

                _healthText.SetText(string.Format("Health: {0:0.0}%, {1:0}/{2:0}", _health.GetHealthPercent(), _health.GetMaxHealth(), _health.CurrentHealth.Value));
            })
            .AddTo(this);
        }
    }
}