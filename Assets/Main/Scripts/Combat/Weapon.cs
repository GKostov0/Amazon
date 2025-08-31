using UnityEngine;

namespace AMAZON.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private AudioSource _onHitAudio;

        public void OnHit()
        {
            if (_onHitAudio)
            {
                _onHitAudio.Play();
                print($"Weapon hit {gameObject.name}");
            }
        }
    }
}