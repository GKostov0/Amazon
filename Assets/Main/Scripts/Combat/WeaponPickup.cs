using AMAZON.Control;
using AMAZON.UI;
using System.Collections;
using UnityEngine;

namespace AMAZON.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] private WeaponSO _weapon = null;
        [SerializeField][Range(1.0f, 20.0f)] private float _respawnTime = 5.0f;
        [SerializeField] private Collider _collider;

        public ECursorType GetCursorType() => ECursorType.Pickup;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Pickup(other.GetComponent<Fighter>());
            }
        }

        private void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(_weapon);
            StartCoroutine(HideForSeconds(_respawnTime));
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

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(callingController.GetComponent<Fighter>());
            }
            return true;
        }
    }
}