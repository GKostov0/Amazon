using UnityEngine;

namespace AMAZON.UI.Inventory
{
    public class InventorySlotUI : MonoBehaviour, IDragContainer<Sprite>
    {
        // CONFIG DATA
        [SerializeField] private InventoryItemIcon _icon = null;

        public int GetNumber() => 1;

        public void RemoveItems(int number) => _icon.SetItem(null);

        public Sprite GetItem() => _icon.GetItem();

        // TODO: make stacks
        public void AddItems(Sprite item, int number) => _icon.SetItem(item);

        // Stacks
        public int MaxAcceptable(Sprite item) => (GetItem() == null) ? int.MaxValue : 0;
    }
}