using AMAZON.Saving;
using AMAZON.UI;
using System;
using System.Collections;
using UnityEngine;

namespace AMAZON.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] private SavingSystem _savingSystem;
        [SerializeField][Range(0.5f, 5.0f)] private float _fadeOutTime = 0.7f;

        private const string _defaultSaveFile = "defaultSave";
        private Fader _fader;

        public void Save() => _savingSystem.Save(_defaultSaveFile);
        public void Load() => _savingSystem.Load(_defaultSaveFile);

        private IEnumerator Start()
        {
            _fader = GameObject.FindGameObjectWithTag("Fader").GetComponent<Fader>();
            _fader.FadeInImmediate();

            yield return _savingSystem.LoadLastScene(_defaultSaveFile);

            _fader.FadeOut(_fadeOutTime);
            yield return new WaitForSeconds(_fadeOutTime);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.F9)) { Load(); }
            if (Input.GetKeyUp(KeyCode.F5)) { Save(); }
        }
    }
}