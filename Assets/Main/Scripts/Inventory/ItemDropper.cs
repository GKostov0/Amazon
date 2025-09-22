using AMAZON.Saving;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace AMAZON.Inventories
{
    public class ItemDropper : MonoBehaviour, ISaveable
    {
        public JToken CaptureAsJToken()
        {
            throw new System.NotImplementedException();
        }

        public void RestoreFromJToken(JToken state)
        {
            throw new System.NotImplementedException();
        }
    }
}