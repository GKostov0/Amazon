using AMAZON.Inventories;
using AMAZON.UI.Inventories;
using AMAZON.UI.Tooltips;
using UnityEngine;

[RequireComponent(typeof(IItemHolder))]
public class ItemTooltipSpawner : TooltipSpawner
{
    private InventoryItem _inventoryItem;
    public override bool CanCreateTooltip() => _inventoryItem != null;

    private void Start()
    {
        _inventoryItem = GetComponent<IItemHolder>().GetItem();
    }

    public override void UpdateTooltip(GameObject tooltip)
    {
        var itemTooltip = tooltip.GetComponent<ItemTooltip>();

        if (!itemTooltip) return;

        var item = GetComponent<IItemHolder>().GetItem();
        itemTooltip.Setup(item);
    }
}