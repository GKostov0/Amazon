using AMAZON.Inventories;
using UnityEngine;

namespace AMAZON.UI.Inventories
{
    public class InventoryDropTarget : MonoBehaviour, IDragDestination<InventoryItem>
    {
        public int MaxAcceptable(InventoryItem item) => int.MaxValue;

        public void AddItems(InventoryItem item, int number)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<ItemDropper>().DropItem(item);
        }
    }
}