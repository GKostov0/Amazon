using UnityEngine;
using AMAZON.Movement;
using AMAZON.Combat;
using AMAZON.Attributes;
using AMAZON.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.AI;

namespace AMAZON.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Mover _playerMover;
        [SerializeField] private Fighter _fighter;
        [SerializeField] private Health _health;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private CursorsSO _cursors;

        [Space(5)]
        [Range(1.0f, 20.0f)]
        [SerializeField] private float _maxNavMeshProjectionDistance = 1.0f;

        [Space(5)]
        [Range(10.0f, 100.0f)]
        [SerializeField] private float _maxNavPathLength = 15.0f;

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
            foreach (RaycastHit hit in RaycastAllSorted())
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                
                foreach(IRaycastable ray in raycastables)
                {
                    if (ray.HandleRaycast(this))
                    {
                        _cursors.SetCursor(ray.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        private RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] result = Physics.RaycastAll(GetMouseRay());
            Array.Sort(result, (a, b) => a.distance.CompareTo(b.distance));
            return result;
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
            Vector3 target;
            bool didHit = RaycastNavmesh(out target);

            if (didHit)
            {
                if (Input.GetMouseButton(0))
                {
                    _playerMover.StartMoveAction(target, 1.0f);
                }
                _cursors.SetCursor(ECursorType.Movement);
                return true;
            }

            return false;
        }

        private bool RaycastNavmesh(out Vector3 target)
        {
            target = Vector3.zero;

            RaycastHit hit;
            bool didHit = Physics.Raycast(GetMouseRay(), out hit);

            if (!didHit) return false;

            // Find nearest point
            NavMeshHit navMeshHit;
            bool result = NavMesh.SamplePosition(hit.point, out navMeshHit, _maxNavMeshProjectionDistance, NavMesh.AllAreas);

            if (!result) return false;

            target = navMeshHit.position;

            // Don't go on rooftops
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;

            // Don't go too far...
            if (GetPathLength(path) > _maxNavPathLength) return false;

            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float result = 0;

            if (path.corners.Length < 2) return result;

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                result += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return result;
        }

        private Ray GetMouseRay()
        {
            return _mainCamera.ScreenPointToRay(Input.mousePosition);
        }
    }
}