using AMAZON.Inventories;
using UnityEngine;

namespace AMAZON.Items.Control
{
    [RequireComponent(typeof(Pickup))]
    public class RunOverPickup : MonoBehaviour
    {
        [SerializeField] private Pickup _pickup;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _pickup.PickupItem();
            }
        }
    }
}