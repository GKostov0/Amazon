using AMAZON.Saving;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace AMAZON.Inventories
{
    public class PickupSpawner : MonoBehaviour, ISaveable
    {
        [SerializeField] private InventoryItem _item = null;

        public bool IsCollected() => GetPickup() == null;

        public Pickup GetPickup() => GetComponentInChildren<Pickup>();

        public JToken CaptureAsJToken() => JToken.FromObject(IsCollected());

        public void RestoreFromJToken(JToken state)
        {
            bool shouldBeCollected = state.ToObject<bool>();

            if (shouldBeCollected && !IsCollected())
            {
                DestroyPickup();
            }

            if (!shouldBeCollected && IsCollected())
            {
                SpawnPickup();
            }
        }

        private void Awake()
        {
            // Spawn in Awake so can be destroyed by save system after.
            SpawnPickup();
        }

        private void SpawnPickup()
        {
            var spawnedPickup = _item.SpawnPickup(transform.position);
            spawnedPickup.transform.SetParent(transform);
        }

        private void DestroyPickup()
        {
            if (GetPickup())
            {
                Destroy(GetPickup().gameObject);
            }
        }
    }
}