using AMAZON.Inventories;
using TMPro;
using UnityEngine;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _titleText;
    [SerializeField] TextMeshProUGUI _bodyText;

    public void Setup(InventoryItem item)
    {
        _titleText.text = item.GetDisplayName();
        _bodyText.text = item.GetDescription();
    }
}