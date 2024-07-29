using System;
using System.Collections.Generic;
using RPG.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace GameDevTV.Saving
{
	[ExecuteAlways]
	public class SaveableEntity : MonoBehaviour
	{
		[SerializeField] private string uniqueIdentifier ="";
		static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();

#if UNITY_EDITOR
		private void Update()
		{
			if(Application.IsPlaying(gameObject))
			{
				return;
			}
			if (string.IsNullOrEmpty(gameObject.scene.path)) return;

			SerializedObject obj = new SerializedObject(this);
			SerializedProperty property = obj.FindProperty("uniqueIdentifier");


			if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
			{
				property.stringValue = System.Guid.NewGuid().ToString();
				obj.ApplyModifiedProperties();
			}

			globalLookup[property.stringValue] = this;
		}
#endif

		private bool IsUnique(string candidate) 
		{
			if (!globalLookup.ContainsKey(candidate)) return true;
			if (globalLookup[candidate] == this) return true;
			if (globalLookup[candidate] == null)
			{
				globalLookup.Remove(candidate);
				return true;
			}

			if (globalLookup[candidate].GetUniqueIdentifier() != candidate) 
			{
				globalLookup.Remove(candidate);
				return true;
			}
			return false;

		}

		public string GetUniqueIdentifier()
		{
			return uniqueIdentifier;
		}

		public object CaptureState() 
		{
			Dictionary<string, object> state = new Dictionary<string, object>();
			foreach (var saveable in GetComponents<ISaveable>())
			{
				state[saveable.GetType().ToString()] = saveable.CaptureState();
			}
			return state;
		}

		public void RestoreState(object state) 
		{
			Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
			foreach (var saveable in GetComponents<ISaveable>())
			{
				if (stateDict.ContainsKey(saveable.GetType().ToString())) 
				{
					saveable.RestoreState(stateDict[saveable.GetType().ToString()]);
				}
			}
		}
	}
}