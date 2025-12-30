using AMAZON.Saving;
using Newtonsoft.Json.Linq;
using UniRx;
using UnityEngine;

namespace AMAZON.Inventories
{

    public class Inventory : MonoBehaviour, ISaveable
    {
        // CONFIG DATA
        [Tooltip("Allowed size")]
        [SerializeField] private int _inventorySize = 16;

        private InventorySlot[] _slots;

        // TODO: update quantity
        public Subject<(InventoryItem, int)> OnInventoryUpdated { get; private set; } = new Subject<(InventoryItem, int)>();

        public bool HasSpaceFor(InventoryItem item) => FindSlot(item) >= 0;
        private int FindSlot(InventoryItem item) => FindEmptySlot();
        public int GetSize() => _slots.Length;
        public InventoryItem GetItemInSlot(int slot) => _slots[slot].Item;

        private void Awake()
        {
            _slots = new InventorySlot[_inventorySize];

            //InventoryItem.GetFromID("fdb5b543-d7a5-43c3-87a5-4f5a67303f8e").SpawnPickup(transform.position);
        }

        public JToken CaptureAsJToken()
        {
            var slotStrings = new string[_inventorySize];
            for (int i = 0; i < _inventorySize; i++)
            {
                if (_slots[i].Item != null)
                {
                    slotStrings[i] = _slots[i].Item.GetItemID();
                }
            }

            return JToken.FromObject(slotStrings);
        }

        public void RestoreFromJToken(JToken state)
        {
            var slotStrings = state.ToObject<string[]>();

            for (int i = 0; i < _inventorySize; i++)
            {
                _slots[i].Item = InventoryItem.GetFromID(slotStrings[i]);
            }

            OnInventoryUpdated.OnNext((null, 1));
        }

        public static Inventory GetPlayerInventory()
        {
            var player = GameObject.FindWithTag("Player");
            return player.GetComponent<Inventory>();
        }

        private int FindEmptySlot()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].Item == null)
                {
                    return i;
                }
            }
            return -1;
        }

        public bool AddItemToSlot(int slot, InventoryItem item, int quantity)
        {
            if (_slots[slot].Item != null)
            {
                return AddToFirstEmptySlot(item, quantity);
            }

            // TODO: update quantity
            // _slots[slot] = item;
            OnInventoryUpdated.OnNext((item, slot));

            return true;
        }

        public bool AddToFirstEmptySlot(InventoryItem item, int quantity)
        {
            int i = FindSlot(item);

            if (i < 0)
            {
                return false;
            }

            // TODO:
            // _slots[i] = item;
            OnInventoryUpdated.OnNext((item, -1));

            return true;
        }

        public bool HasItem(InventoryItem item)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (ReferenceEquals(_slots[i].Item, item))
                {
                    return true;
                }
            }

            return false;
        }

        public void RemoveFromSlot(int slot, int quantity)
        {
            _slots[slot].Item = null;
            // TODO: update quantity
            OnInventoryUpdated.OnNext((null, slot));
        }
    }
}