using AMAZON.Saving;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AMAZON.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [ReadOnly][SerializeField] private float _experiencePoints = 0.0f;

        public JToken CaptureAsJToken() => JToken.FromObject(_experiencePoints);
        public void RestoreFromJToken(JToken state) => _experiencePoints = state.ToObject<float>();
        public void GainExperience(float xp) => _experiencePoints += xp;
    }
}