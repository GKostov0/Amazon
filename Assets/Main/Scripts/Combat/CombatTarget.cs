using UnityEngine;
using AMAZON.Attributes;
using AMAZON.Control;
using AMAZON.UI;

namespace AMAZON.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public ECursorType GetCursorType() => ECursorType.Combat;

        public bool HandleRaycast(PlayerController callingController)
        {
            Fighter fighter = callingController.GetComponent<Fighter>();

            if (!fighter.CanAttack(gameObject)) return false;

            if (Input.GetMouseButton(0))
            {
                fighter.Attack(gameObject);
            }

            return true;
        }
    }
}