using AMAZON.Combat;
using AMAZON.Core;
using AMAZON.Movement;
using AMAZON.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;
using UniRx;
using Sirenix.Utilities;


namespace AMAZON.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private Fighter _fighter;
        [SerializeField] private Health _health;
        [SerializeField] private Mover _mover;
        [SerializeField] private ActionScheduler _actionScheduler;
        [SerializeField] private PatrolPath _patrolPath;
        [SerializeField][Range(0.0f, 20.0f)] private float _suspicionTime = 4.0f;
        [SerializeField][Range(0.0f, 20.0f)] private float _aggroTime = 5.0f;
        [SerializeField][Range(0.0f, 50.0f)] private float _shoutDistance = 5.0f;
        [SerializeField][Range(0.0f, 20.0f)] private float _waypointTolerance = 1.0f;

        [InfoBox("Min Max Dwelling Time")]
        [MinMaxSlider(0, 20, true)]
        [SerializeField] private Vector2 _dwellingTime;

        [SerializeField][Range(0.0f, 1.0f)] private float _patrolSpeedFraction = 0.2f;

        private PlayerController _playerController;

        private Vector3 _startingPosition;

        private float _timeSinceLastSawPlayer = Mathf.Infinity;
        private float _timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private float _timeSinceAggravated = Mathf.Infinity;

        private float _currentDwellTime;
        private int _currentWaypointIndex = 0;

        private void Awake()
        {
            _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _currentDwellTime = Random.Range(_dwellingTime.x, _dwellingTime.y);
        }

        private void Start()
        {
            _startingPosition = transform.position;

            _health.OnTakeDamage.Subscribe(damage => 
            {
                Aggravate();
            })
            .AddTo(this);
        }

        [SerializeField][Range(1.0f, 50.0f)] private float _chaseDistance = 5.0f;

        private void Update()
        {
            if (_health.IsDead()) return;

            if (IsAggravated() && _fighter.CanAttack(_playerController.gameObject))
            {
                AttackBehavour();
            }
            else if (_timeSinceLastSawPlayer < _suspicionTime)
            {
                SuspicionBehavour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        private void Aggravate()
        {
            _timeSinceAggravated = 0;
        }

        private void UpdateTimers()
        {
            _timeSinceLastSawPlayer += Time.deltaTime;
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
            _timeSinceAggravated += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = _startingPosition;

            if (_patrolPath != null)
            {
                if (IsAtWaypoint())
                {
                    CycleWaypoint();

                    _timeSinceArrivedAtWaypoint = 0;
                    _currentDwellTime = Random.Range(_dwellingTime.x, _dwellingTime.y);
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (_timeSinceArrivedAtWaypoint >= _currentDwellTime)
            {
                _mover.StartMoveAction(nextPosition, _patrolSpeedFraction);
            }
        }

        private void SuspicionBehavour()
        {
            _actionScheduler.CancelCurrentAction();
        }

        private void AttackBehavour()
        {
            _timeSinceLastSawPlayer = 0;
            _fighter.Attack(_playerController.gameObject);

            AggravateNearbyEnemies();
        }

        private void AggravateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, _shoutDistance, Vector3.up, 0.0f);

            hits.ForEach(x =>
            {
                if (x.collider.TryGetComponent<AIController>(out var enemy))
                {
                    enemy.Aggravate();
                }
            });
        }

        private bool IsAtWaypoint()
        {
            return Vector3.Distance(transform.position, GetCurrentWaypoint()) < _waypointTolerance;
        }

        private Vector3 GetCurrentWaypoint()
        {
            return _patrolPath.GetWaypoint(_currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            _currentWaypointIndex = (_patrolPath.transform.childCount - 1) == _currentWaypointIndex ? 0 : ++_currentWaypointIndex;
        }

        private bool IsAggravated()
        {
            float dstanceToPlayer = Vector3.Distance(_playerController.transform.position, transform.position);
            return dstanceToPlayer <= _chaseDistance || _timeSinceAggravated < _aggroTime;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _chaseDistance);
        }
    }
}