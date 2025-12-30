using UnityEngine;

namespace AMAZON.Inventories
{
    public class Pickup : MonoBehaviour
    {
        private InventoryItem _item;
        private int _quantity;

        private Inventory _inventory;

        public void Setup(InventoryItem item, int quantity)
        {
            _item = item;
            // TODO: add quantity
        }

        public InventoryItem GetItem() => _item;

        public bool CanBePickedUp() => _inventory.HasSpaceFor(_item);

        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            _inventory = player.GetComponent<Inventory>();
        }

        public void PickupItem()
        {
            // TODO: add quantity
            bool foundSlot = _inventory.AddToFirstEmptySlot(_item, 1);
            if (foundSlot)
            {
                Destroy(gameObject);
            }
        }
    }
}