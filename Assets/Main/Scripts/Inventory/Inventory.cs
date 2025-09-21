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

        private InventoryItem[] _slots;

        public Subject<(InventoryItem, int)> OnInventoryUpdated { get; private set; } = new Subject<(InventoryItem, int)>();

        public bool HasSpaceFor(InventoryItem item) => FindSlot(item) >= 0;
        private int FindSlot(InventoryItem item) => FindEmptySlot();
        public int GetSize() => _slots.Length;
        public InventoryItem GetItemInSlot(int slot) => _slots[slot];

        private void Awake()
        {
            _slots = new InventoryItem[_inventorySize];

            //InventoryItem.GetFromID("a9f68ac1-4737-4049-bd4f-58106c8bd53a").SpawnPickup(transform.position);
        }

        public JToken CaptureAsJToken()
        {
            var slotStrings = new string[_inventorySize];
            for (int i = 0; i < _inventorySize; i++)
            {
                if (_slots[i] != null)
                {
                    slotStrings[i] = _slots[i].GetItemID();
                }
            }

            return JToken.FromObject(slotStrings);
        }

        public void RestoreFromJToken(JToken state)
        {
            var slotStrings = state.ToObject<string[]>();

            for (int i = 0; i < _inventorySize; i++)
            {
                _slots[i] = InventoryItem.GetFromID(slotStrings[i]);
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
                if (_slots[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        public bool AddItemToSlot(int slot, InventoryItem item)
        {
            if (_slots[slot] != null)
            {
                return AddToFirstEmptySlot(item);
            }

            _slots[slot] = item;
            OnInventoryUpdated.OnNext((item, slot));

            return true;
        }

        public bool AddToFirstEmptySlot(InventoryItem item)
        {
            int i = FindSlot(item);

            if (i < 0)
            {
                return false;
            }

            _slots[i] = item;
            OnInventoryUpdated.OnNext((item, -1));

            return true;
        }

        public bool HasItem(InventoryItem item)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (ReferenceEquals(_slots[i], item))
                {
                    return true;
                }
            }

            return false;
        }

        public void RemoveFromSlot(int slot)
        {
            _slots[slot] = null;
            OnInventoryUpdated.OnNext((null, slot));
        }
    }
}