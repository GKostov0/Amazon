using AMAZON.Combat;
using System;
using TMPro;
using UniRx;
using UnityEngine;

namespace AMAZON.UI
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _enemyHealthText;

        private Fighter _playerFighter;

        private CompositeDisposable _disposible;

        private void Start()
        {
            _playerFighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            _disposible = new CompositeDisposable();

            _playerFighter.Target.Subscribe(newValue => 
            {
                if (newValue)
                {
                    _disposible.Clear();
                    _disposible = new CompositeDisposable();

                    newValue.CurrentHealth.Subscribe(NewHealth => 
                    {
                        _enemyHealthText.SetText(string.Format("Target: {0:0.0}%", newValue.GetHealthPercent()));
                    })
                    .AddTo(_disposible);
                }
                else
                {
                    _disposible.Clear();
                    _enemyHealthText.SetText("[-]");
                }

            })
            .AddTo(this);
        }
    }
}