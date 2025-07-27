using UnityEngine;

namespace AMAZON.Audio
{
    public class AudioRandomizer : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _audioClips;
        [SerializeField][Range(0.0f, 1.0f)] private float _chanceToPlay;
        [SerializeField] private AudioSource _audioSource;

        public void PlaySound()
        {
            if (_audioSource && (_audioClips.Length > 0))
            {
                if (Random.value < _chanceToPlay)
                {
                    _audioSource.clip = _audioClips[Random.Range(0, _audioClips.Length - 1)];
                    _audioSource.Play();
                }
            }
            else
            {
                Debug.LogWarning("Trying to play a sound but some conditions are not met... ignoring....");
            }
        }
    }
}