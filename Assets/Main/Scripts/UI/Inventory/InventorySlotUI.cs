using AMAZON.Inventories;
using UnityEngine;

namespace AMAZON.UI.Inventories
{
    public class InventorySlotUI : MonoBehaviour, IDragContainer<InventoryItem>
    {
        // CONFIG DATA
        [SerializeField] private InventoryItemIcon _icon = null;

        private int _index;
        private Inventory _inventory;

        public int GetNumber() => 1;

        public void AddItems(InventoryItem item, int number) => _inventory.AddItemToSlot(_index, item);

        public void RemoveItems(int number) => _inventory.RemoveFromSlot(_index);

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