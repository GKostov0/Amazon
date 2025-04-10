using UnityEngine;
using UnityEngine.Playables;

namespace AMAZON.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        [SerializeField] private PlayableDirector _playableDirector;

        private bool _didCinematicPlay;

        private void OnTriggerEnter(Collider other)
        {
            if (!_didCinematicPlay && other.CompareTag("Player"))
            {
                _didCinematicPlay = true;
                _playableDirector.Play();
            }
        }
    }
}