using System.Collections;
using UnityEngine;

namespace AMAZON.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] private WeaponSO _weapon = null;
        [SerializeField][Range(1.0f, 20.0f)] private float _respawnTime = 5.0f;
        [SerializeField] private Collider _collider;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Fighter>().EquipWeapon(_weapon);
                StartCoroutine(HideForSeconds(_respawnTime));
            }
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            SetPickupState(false);
            yield return new WaitForSeconds(seconds);
            SetPickupState(true);
        }

        private void SetPickupState(bool visible)
        {
            _collider.enabled = visible;

            foreach (Transform child in transform)
                child.gameObject.SetActive(visible);
        }
    }
}