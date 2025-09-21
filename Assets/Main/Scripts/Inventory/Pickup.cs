using UnityEngine;

namespace AMAZON.Inventories
{
    public class Pickup : MonoBehaviour
    {
        private InventoryItem _item;

        private Inventory _inventory;

        public void Setup(InventoryItem item) => _item = item;

        public InventoryItem GetItem() => _item;

        public bool CanBePickedUp() => _inventory.HasSpaceFor(_item);

        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            _inventory = player.GetComponent<Inventory>();
        }

        public void PickupItem()
        {
            bool foundSlot = _inventory.AddToFirstEmptySlot(_item);
            if (foundSlot)
            {
                Destroy(gameObject);
            }
        }
    }
}