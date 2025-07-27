using AMAZON.Control;
using AMAZON.UI;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace AMAZON.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private int _sceneToLoad = -1;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private EPortalDestination _portalID;

        [InfoBox("FadeIn FadeOut Time")]
        [MinMaxSlider(0.1f, 10.0f, true)]
        [SerializeField] private Vector2 _fadeInOutTime;

        private Fader _fader;
        private SavingWrapper _savingWrapper;

        private void Start()
        {
            _fader = GameObject.FindGameObjectWithTag("Fader").GetComponent<Fader>();
            _savingWrapper = GameObject.FindGameObjectWithTag("Save").GetComponent<SavingWrapper>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(TransitionLevel());
            }
        }

        private IEnumerator TransitionLevel()
        {
            if (_sceneToLoad < 0)
            {
                Debug.LogError("Scene to load is not set!");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            PlayerController playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = false;

            _fader.FadeIn(_fadeInOutTime.y);
            yield return new WaitForSeconds(_fadeInOutTime.y);

            _savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(_sceneToLoad);

            PlayerController newPlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            newPlayerController.enabled = false;

            _savingWrapper.Load();

            Debug.Log($"Level Loaded: {_sceneToLoad}");
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            _savingWrapper.Save();
            _fader.FadeOut(_fadeInOutTime.x);

            newPlayerController.enabled = true;
            yield return new WaitForSeconds(_fadeInOutTime.x);

            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsByType<Portal>(FindObjectsSortMode.None))
            {
                if (portal == this || portal._portalID != _portalID) continue;
                return portal;
            }

            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            PlayerController playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            NavMeshAgent playerNavMeshAgent = playerController.GetComponent<NavMeshAgent>();

            playerNavMeshAgent.enabled = false;

            playerController.transform.position = otherPortal._spawnPoint.position;

            playerNavMeshAgent.enabled = true;
        }
    }
}