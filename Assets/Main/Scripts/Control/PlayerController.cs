using UnityEngine;
using AMAZON.Movement;
using AMAZON.Combat;
using AMAZON.Attributes;

namespace AMAZON.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Mover _playerMover;
        [SerializeField] private Fighter _fighter;
        [SerializeField] private Health _health;
        [SerializeField] private Camera _mainCamera;

        private void Update()
        {
            if (_health.IsDead()) return;

            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithMovement()
        {
            if (Physics.Raycast(GetMouseRay(), out RaycastHit hit))
            {
                if (Input.GetMouseButton(0))
                {
                    _playerMover.StartMoveAction(hit.point, 1.0f);
                }

                return true;
            }

            return false;
        }

        private bool InteractWithCombat()
        {
            foreach (RaycastHit hit in Physics.RaycastAll(GetMouseRay()))
            {
                hit.transform.TryGetComponent<CombatTarget>(out var ct);
                if (ct == null) continue;

                if (!_fighter.CanAttack(ct.gameObject)) continue;

                if (Input.GetMouseButton(0))
                {
                    _fighter.Attack(ct.gameObject);
                }
                return true;
            }

            return false;
        }

        private Ray GetMouseRay()
        {
            return _mainCamera.ScreenPointToRay(Input.mousePosition);
        }
    }
}