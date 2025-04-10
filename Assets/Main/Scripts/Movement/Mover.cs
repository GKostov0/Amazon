using AMAZON.Core;
using UnityEngine;
using UnityEngine.AI;
using AMAZON.Saving;
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

        public JToken CaptureAsJToken() => transform.position.ToToken();
        public void Cancel() => _navMeshAgent.isStopped = true;

        private void Update()
        {
            _navMeshAgent.enabled = !_health.IsDead();

            UpdateAnimator();
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