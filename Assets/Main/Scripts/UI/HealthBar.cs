using AMAZON.Attributes;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthImage;
    [SerializeField] private Health _health;

    private void Start()
    {
        _health.CurrentHealth.Subscribe(_ => 
        {
            _healthImage.fillAmount = _health.GetHealthFraction();
        })
        .AddTo(this);
    }
}