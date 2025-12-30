using AMAZON.Inventories;
using UnityEngine;

namespace AMAZON.UI.Inventories
{
    public class InventorySlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        // CONFIG DATA
        [SerializeField] private InventoryItemIcon _icon = null;

        private int _index;
        private Inventory _inventory;

        public int GetNumber() => 1;

        public void AddItems(InventoryItem item, int quantity) => _inventory.AddItemToSlot(_index, item, quantity);

        public void RemoveItems(int quantity) => _inventory.RemoveFromSlot(_index, quantity);

        public InventoryItem GetItem() => _inventory.GetItemInSlot(_index);

        public void Setup(Inventory inventory, int index)
        {
            _inventory = inventory;
            _index = index;
            _icon.SetItem(_inventory.GetItemInSlot(index));
        }

        // Stacks
        public int MaxAcceptable(InventoryItem item)
        {
            if (_inventory.HasSpaceFor(item))
            {
                return int.MaxValue;
            }
            return 0;
        }
    }
}