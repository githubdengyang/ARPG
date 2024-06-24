using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
namespace RPG.Attribute
{
	public class Experience : MonoBehaviour, ISaveable
	{
		[SerializeField ]float experiencePoints = 0;

		public void GainExpeience(float experience)
		{
			experiencePoints += experience;
		}

		public object CaptureState()
		{
			return experiencePoints;
		}

		public void RestoreState(object state)
		{
			experiencePoints = (float)state;
		}

		public float ExperiencePoints() 
		{
			return experiencePoints;
		}
	}
}
