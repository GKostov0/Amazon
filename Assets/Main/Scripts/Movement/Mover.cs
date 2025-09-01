using AMAZON.Core;
using AMAZON.Saving;
using AMAZON.Attributes;
using UnityEngine;
using UnityEngine.AI;
using Newtonsoft.Json.Linq;

namespace AMAZON.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Animator _animator;
        [SerializeField] private ActionScheduler _actionScheduler;
        [SerializeField] private Health _health;

        [SerializeField][Range(1.0f, 50.0f)] private float _maxSpeed = 6.0f;

        [Space(5)]
        [Range(10.0f, 100.0f)]
        [SerializeField] private float _maxNavPathLength = 15.0f;

        public JToken CaptureAsJToken() => transform.position.ToToken();
        public void Cancel() => _navMeshAgent.isStopped = true;

        private void Update()
        {
            _navMeshAgent.enabled = !_health.IsDead();

            UpdateAnimator();
        }

        public bool CanMoveTo(Vector3 destination)
        {
            // Don't go on rooftops
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath) { return false; }
            if (path.status != NavMeshPathStatus.PathComplete) { return false; }

            // Don't go too far...
            if (GetPathLength(path) > _maxNavPathLength) { return false; }

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

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            _actionScheduler.StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.speed = _maxSpeed * Mathf.Clamp01(speedFraction);
            _navMeshAgent.destination = destination;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = transform.InverseTransformDirection(_navMeshAgent.velocity);
            _animator.SetFloat("forwardSpeed", velocity.z);
        }

        public void RestoreFromJToken(JToken state)
        {
            _navMeshAgent.enabled = false;
            transform.position = state.ToVector3();
            _navMeshAgent.enabled = true;
            _actionScheduler.CancelCurrentAction();
        }
    }
}