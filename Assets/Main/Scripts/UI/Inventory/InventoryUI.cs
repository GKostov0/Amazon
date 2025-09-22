using UnityEngine;
using UniRx;
using AMAZON.Inventories;

namespace AMAZON.UI.Inventories
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private InventorySlotUI _inventoryItemPrefab = null;
        [SerializeField] private Transform _itemsContainer = null;

        private Inventory _playerInventory;

        private void Awake()
        {
            _playerInventory = Inventory.GetPlayerInventory();

            // Tuple for Item1 - InventoryItem and Item2 - slot position
            _playerInventory.OnInventoryUpdated.Subscribe(newItem =>
            {
                if (newItem.Item1 != null || newItem.Item2 >= 0)
                {
                    Redraw();
                }
            })
            .AddTo(this);
        }

        private void Start()
        {
            Redraw();
        }

        private void Redraw()
        {
            foreach (Transform child in _itemsContainer)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < _playerInventory.GetSize(); i++)
            {
                var itemUI = Instantiate(_inventoryItemPrefab, _itemsContainer);
                itemUI.Setup(_playerInventory, i);
            }
        }
    }
}