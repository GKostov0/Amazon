using UnityEngine;

namespace AMAZON.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] private KeyCode _toggleKey = KeyCode.Escape;
        [SerializeField] private GameObject _uiContainer = null;

        private void Start()
        {
            _uiContainer.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(_toggleKey))
            {
                _uiContainer.SetActive(!_uiContainer.activeSelf);
            }
        }
    }
}