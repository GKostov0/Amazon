using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace AMAZON.UI
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public void FadeIn(float fadeInTime) => _image.DOFade(1.0f, fadeInTime).From(0.0f);
        public void FadeOut(float fadeOutTime) => _image.DOFade(0.0f, fadeOutTime).From(1.0f);
        public void FadeInImmediate() => _image.color = Color.white;
    }
}