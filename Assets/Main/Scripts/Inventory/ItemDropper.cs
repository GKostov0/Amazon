using AMAZON.Saving;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AMAZON.Inventories
{
    public class ItemDropper : MonoBehaviour, ISaveable
    {
        private List<Pickup> _droppedItems = new List<Pickup>();

        protected virtual Vector3 GetDropLocation() => transform.position + new Vector3(Random.Range(0.5f, 1.0f), 0.0f, Random.Range(0.5f, 1.0f));

        public JToken CaptureAsJToken()
        {
            RemoveDestroyedDrops();

            var droppedItemsList = new DropRecord[_droppedItems.Count];
            for (int i = 0; i < droppedItemsList.Length; i++)
            {
                droppedItemsList[i].itemID = _droppedItems[i].GetItem().GetItemID();
                droppedItemsList[i].position = new SerializableVector3(_droppedItems[i].transform.position);
            }
            return JToken.FromObject(droppedItemsList);
        }

        public void RestoreFromJToken(JToken state)
        {
            var droppedItemsList = state.ToObject<DropRecord[]>();
            foreach (var item in droppedItemsList)
            {
                var pickupItem = InventoryItem.GetFromID(item.itemID);
                Vector3 position = item.position.ToVector();
                SpawnPickup(pickupItem, position);
            }
        }

        public void DropItem(InventoryItem item)
        {
            SpawnPickup(item, GetDropLocation());
        }

        public void SpawnPickup(InventoryItem item, Vector3 spawnLocation)
        {
            var pickup = item.SpawnPickup(spawnLocation);
            _droppedItems.Add(pickup);
        }

        private void RemoveDestroyedDrops()
        {
            var newList = new List<Pickup>();
            foreach (var item in _droppedItems)
            {
                if (item != null)
                {
                    newList.Add(item);
                }
            }
            _droppedItems = newList;
        }

        [System.Serializable]
        private struct DropRecord
        {
            public string itemID;
            public SerializableVector3 position;
        }
    }
}