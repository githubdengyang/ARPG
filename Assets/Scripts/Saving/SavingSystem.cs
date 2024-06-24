using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
	public class SavingSystem : MonoBehaviour
	{
		public void Save(string saveFile) 
		{
			Dictionary<string, object> state = LoadFile(saveFile);
			CaptureState(state);
			state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
			SaveFile(saveFile, state);
		}

		private void SaveFile(string saveFile, Dictionary<string, object> state)
		{
			string path = GetPathFromSaveFile(saveFile);
			Debug.Log("Saving to " + path);
			using (FileStream stream = File.Open(path, FileMode.Create))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, state);
			}
		}


		public void Load(string loadFile)
		{
			RestoreState(LoadFile(loadFile));
		}

		private Dictionary<string, object> LoadFile(string loadFile) 
		{
			string path = GetPathFromSaveFile(loadFile);
			Debug.Log("Loading from " + GetPathFromSaveFile(loadFile));
			if (!File.Exists(path)) return new Dictionary<string, object>();

			using (FileStream stream = File.Open(path, FileMode.Open))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				return (Dictionary<string, object>)formatter.Deserialize(stream);
			}
		}

		private void CaptureState(Dictionary<string, object> state) 
		{
			foreach (SaveableEntity item in FindObjectsOfType<SaveableEntity>()) 
			{
				state[item.GetUniqueIdentifier()] = item.CaptureState();
			}
		}

		private void RestoreState(Dictionary<string, object> stateDict) 
		{
			foreach (SaveableEntity item in FindObjectsOfType<SaveableEntity>())
			{
				if (!stateDict.ContainsKey(item.GetUniqueIdentifier())) continue;
				item.RestoreState(stateDict[item.GetUniqueIdentifier()]);
			}
		}



		private string GetPathFromSaveFile(string saveFile)
		{
			return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
		}

		public IEnumerator LoadLastScene(string saveFile) 
		{
			Dictionary<string, object> state = LoadFile(saveFile);
			if (state.ContainsKey("lastSceneBuildIndex"))
			{
				int buildIndex = (int)state["lastSceneBuildIndex"];
				if (buildIndex != SceneManager.GetActiveScene().buildIndex)
				{
					yield return SceneManager.LoadSceneAsync(buildIndex);
				}
			}
			RestoreState(state);

		}
	}
}