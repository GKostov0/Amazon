using System.Collections.Generic;
using UnityEngine;

namespace AMAZON.Inventories
{

    [CreateAssetMenu(menuName = "Inventory/NewItem")]
    public class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
    {
        // CONFIG DATA
        [Tooltip("Auto-generated UUID for saving/loading. Clear this field if you want to generate a new one.")]
        [SerializeField] private string _itemID = null;

        [Tooltip("Item name to be displayed in UI.")]
        [SerializeField] private string _displayName = null;

        [Tooltip("Item description to be displayed in UI.")]
        [SerializeField][TextArea] private string _description = null;

        [Tooltip("The UI icon to represent this item in the inventory.")]
        [SerializeField] private Sprite _icon = null;

        [Tooltip("The prefab that should be spawned when this item is dropped.")]
        [SerializeField] private Pickup _pickup = null;

        [Tooltip("If true, multiple items of this type can be stacked in the same inventory slot.")]
        [SerializeField] private bool _isStackable = false;

        private static Dictionary<string, InventoryItem> _itemLookupCache;

        public string GetItemID() => _itemID;
        public Sprite GetIcon() => _icon;

        public void OnBeforeSerialize()
        {
            // Generate and save a new UUID if this is blank.
            if (string.IsNullOrWhiteSpace(_itemID))
            {
                _itemID = System.Guid.NewGuid().ToString();
            }
        }

        public void OnAfterDeserialize()
        {

        }

        public Pickup SpawnPickup(Vector3 position)
        {
            var pickup = Instantiate(_pickup);
            pickup.transform.position = position;
            pickup.Setup(this);
            return pickup;
        }

        public static InventoryItem GetFromID(string itemID)
        {
            if (_itemLookupCache == null)
            {
                _itemLookupCache = new Dictionary<string, InventoryItem>();

                var itemList = Resources.LoadAll<InventoryItem>("");
                foreach (var item in itemList)
                {
                    if (_itemLookupCache.ContainsKey(item._itemID))
                    {
                        Debug.LogError($"[Error]: Duplicate UI InventorySystem ID for objects: {_itemLookupCache[item._itemID]} and {item}");
                        continue;
                    }

                    _itemLookupCache[item._itemID] = item;
                }
            }

            if (itemID == null || !_itemLookupCache.ContainsKey(itemID)) return null;
            return _itemLookupCache[itemID];
        }
    }
}