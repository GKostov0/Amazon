using AMAZON.Saving;
using Newtonsoft.Json.Linq;
using UniRx;
using UnityEngine;

namespace AMAZON.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [HideInInspector] public ReactiveProperty<bool> OnRestoreComplete = new ReactiveProperty<bool>(false);

        public ReactiveProperty<float> ExperiencePoints { get; private set; } = new ReactiveProperty<float>();

        public JToken CaptureAsJToken() => JToken.FromObject(ExperiencePoints.Value);
        
        public void GainExperience(float xp) => ExperiencePoints.Value += xp;

        public void RestoreFromJToken(JToken state)
        {
            ExperiencePoints.Value = state.ToObject<float>();
            OnRestoreComplete.Value = true;
        }
    }
}