using AMAZON.Attributes;
using AMAZON.Stats;
using TMPro;
using UniRx;
using UnityEngine;

namespace AMAZON.UI
{

    public class PlayerExperienceDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _experienceText;

        private Experience _playerExperience;

        private void Start()
        {
            _playerExperience = GameObject.FindWithTag("Player").GetComponent<Experience>();

            _playerExperience.ExperiencePoints.Subscribe(newValue => 
            {
                _experienceText.SetText(newValue.ToString());
            })
            .AddTo(this);
        }

    }
}