using UnityEngine;
using UnityEngine.UI;

namespace AMAZON.UI.Inventory
{
    [RequireComponent(typeof(Image))]
    public class InventoryItemIcon : MonoBehaviour
    {
        [SerializeField] private Image _iconImage = null;

        public Sprite GetItem() => _iconImage.enabled ? _iconImage.sprite : null;

        public void SetItem(Sprite item)
        {
            if (item == null)
            {
                _iconImage.enabled = false;
            }
            else
            {
                _iconImage.enabled = true;
                _iconImage.sprite = item;
            }
        }
    }
}