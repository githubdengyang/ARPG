using RPG.Core;
using GameDevTV.Saving;
using RPG.Stats;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Events;
using System;
namespace RPG.Stats
{
	public class Experience : MonoBehaviour, ISaveable
	{
		[SerializeField ]float experiencePoints = 0;
		public Action onExperienceGained;
		public void GainExpeience(float experience)
		{
			experiencePoints += experience;
			onExperienceGained();
		}

		public object CaptureState()
		{
			return experiencePoints;
		}

		public void RestoreState(object state)
		{
			experiencePoints = (float)state;
		}

		public float GetPoints() 
		{
			return experiencePoints;
		}
	}
}
