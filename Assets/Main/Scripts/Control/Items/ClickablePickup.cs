using AMAZON.Control;
using AMAZON.Inventories;
using AMAZON.UI;
using UnityEngine;

namespace AMAZON.Items.Control
{
    [RequireComponent(typeof(Pickup))]
    public class ClickablePickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] private Pickup _pickup;

        public ECursorType GetCursorType() => _pickup.CanBePickedUp() ? ECursorType.Pickup : ECursorType.FullPickup;

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _pickup.PickupItem();
            }
            return true;
        }
    }
}