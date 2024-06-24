using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using Unity.Jobs;
using static Cinemachine.DocumentationSortingAttribute;

namespace RPG.Stats
{
	public class BaseStats : MonoBehaviour
	{
		[Range(1, 99)]
		[SerializeField] int startingLevel = 1;
		[SerializeField] CharacterClass characterClass;
		[SerializeField] Progression progression = null;

		public float GetHealth()
		{
			return progression.GetStat(Stat.Health, characterClass, startingLevel);
		}

		public float GetExperienceReward()
		{
			return progression.GetStat(Stat.ExperienceReward, characterClass, startingLevel);
		}
	}
}
