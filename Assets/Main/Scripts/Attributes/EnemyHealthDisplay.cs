using AMAZON.Combat;
using System.Collections;
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

        private void Awake()
        {
            _playerFighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            _disposible = new CompositeDisposable();
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(2.0f);

            _playerFighter.Target.Subscribe(newValue => 
            {
                if (newValue)
                {
                    _disposible.Clear();
                    _disposible = new CompositeDisposable();

                    newValue.CurrentHealth.Subscribe(NewHealth => 
                    {
                        _enemyHealthText.SetText(string.Format("Target: {0:0.0}% {1:0}/{2:0}", newValue.GetHealthPercent(), newValue.GetMaxHealth(), newValue.CurrentHealth.Value));
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