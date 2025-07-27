using AMAZON.Attributes;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthImage;
    [SerializeField] private Health _health;
    [SerializeField] private GameObject _healthCanvas;

    private void Start()
    {
        _health.NormalizedHealth.Subscribe(newValue =>
        {
            if (Mathf.Approximately(newValue, 0) || Mathf.Approximately(newValue, 1))
            {
                _healthCanvas.SetActive(false);
                return;
            }

            _healthCanvas.SetActive(true);
            _healthImage.fillAmount = newValue;
        })
        .AddTo(this);
    }
}