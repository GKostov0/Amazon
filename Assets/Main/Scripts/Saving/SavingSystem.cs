using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace AMAZON.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        private const string _extension = ".json";

        private string GetPathFromSaveFile(string saveFile) => Path.Combine(Application.persistentDataPath, saveFile + _extension);

        public IEnumerator LoadLastScene(string saveFile)
        {
            JObject state = LoadFromFile(saveFile);
            IDictionary<string, JToken> stateDict = state;

            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            if (stateDict.ContainsKey("lastSceneBuildIndex"))
            {
                buildIndex = (int)stateDict["lastSceneBuildIndex"];
            }

            if (buildIndex.Equals(SceneManager.GetActiveScene().buildIndex))
            {
                yield return null;
            }
            else
            {
                yield return SceneManager.LoadSceneAsync(buildIndex);
            }

            RestoreFromToken(state);
        }

        public void Save(string saveFile)
        {
            JObject state = LoadFromFile(saveFile);
            CaptureAsToken(state);
            SaveFile(saveFile, state);
        }

        public void Load(string saveFile)
        {
            RestoreFromToken(LoadFromFile(saveFile));
        }

        public void Delete(string saveFile)
        {
            File.Delete(GetPathFromSaveFile(saveFile));
        }

        public IEnumerable<string> ListSaves()
        {
            foreach (string path in Directory.EnumerateFiles(Application.persistentDataPath))
            {
                if (Path.GetExtension(path) == _extension)
                {
                    yield return Path.GetFileNameWithoutExtension(path);
                }
            }
        }

        private JObject LoadFromFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                return new JObject();
            }

            using (var textReader = File.OpenText(path))
            {
                using (var reader = new JsonTextReader(textReader))
                {
                    reader.FloatParseHandling = FloatParseHandling.Double;

                    return JObject.Load(reader);
                }
            }
        }


        private void SaveFile(string saveFile, JObject state)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);

            using (var textWriter = File.CreateText(path))
            {
                using (var writer = new JsonTextWriter(textWriter))
                {
                    writer.Formatting = Formatting.Indented;
                    state.WriteTo(writer);
                }
            }
            PlayerPrefs.SetInt("save", 1);
        }

        private void CaptureAsToken(JObject state)
        {
            IDictionary<string, JToken> stateDict = state;
            foreach (SaveableEntity saveable in FindObjectsByType<SaveableEntity>(FindObjectsSortMode.None))
            {
                stateDict[saveable.GetUniqueIdentifier()] = saveable.CaptureAsJtoken();
            }

            stateDict["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }


        private void RestoreFromToken(JObject state)
        {
            IDictionary<string, JToken> stateDict = state;
            foreach (SaveableEntity saveable in FindObjectsByType<SaveableEntity>(FindObjectsSortMode.None))
            {
                string id = saveable.GetUniqueIdentifier();
                if (stateDict.ContainsKey(id))
                {
                    saveable.RestoreFromJToken(stateDict[id]);
                }
            }
        }
    }
}