using UnityEngine;

namespace AMAZON.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] private WeaponSO _weapon = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Fighter>().EquipWeapon(_weapon);
                Destroy(gameObject);
            }
        }
    }
}