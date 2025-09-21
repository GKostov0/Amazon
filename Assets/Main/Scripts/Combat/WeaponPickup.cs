using AMAZON.Attributes;
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
        [SerializeField][Range(0.0f, 1000.0f)] private float _healthToRestore = 0.0f;
        [SerializeField] private Collider _collider;

        public ECursorType GetCursorType() => ECursorType.Pickup;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Pickup(other.gameObject);
            }
        }

        private void Pickup(GameObject subject)
        {
            if (_weapon != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(_weapon);
            }

            if (_healthToRestore > 0.0f)
            {
                subject.GetComponent<Health>().Heal(_healthToRestore);
            }

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
                Pickup(callingController.gameObject);
            }
            return true;
        }
    }
}