using UnityEngine;
using AMAZON.Movement;
using AMAZON.Combat;
using AMAZON.Attributes;
using AMAZON.UI;
using UnityEngine.EventSystems;

namespace AMAZON.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Mover _playerMover;
        [SerializeField] private Fighter _fighter;
        [SerializeField] private Health _health;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private CursorsSO _cursors;

        private void Update()
        {
            if (InteractWithUI())return;

            if (_health.IsDead())
            {
                _cursors.SetCursor(ECursorType.None);
                return;
            }

            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;

            _cursors.SetCursor(ECursorType.None);
        }

        private bool InteractWithComponent()
        {
            foreach (RaycastHit hit in Physics.RaycastAll(GetMouseRay()))
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                
                foreach(IRaycastable ray in raycastables)
                {
                    if (ray.HandleRaycast(this))
                    {
                        _cursors.SetCursor(ECursorType.Pickup);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                _cursors.SetCursor(ECursorType.UI);
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            if (Physics.Raycast(GetMouseRay(), out RaycastHit hit))
            {
                if (Input.GetMouseButton(0))
                {
                    _playerMover.StartMoveAction(hit.point, 1.0f);
                }
                _cursors.SetCursor(ECursorType.Movement);
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