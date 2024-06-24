using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using Unity.Jobs;

namespace RPG.Stats
{
	[CreateAssetMenu(fileName = "Progression", menuName = "Stats/Progression", order = 0)]
	public class Progression : ScriptableObject
	{
		public ProgressionCharacterClass[] characterClasses = null;

		Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

		public float GetStat(Stat stat, CharacterClass characterClass, int level)
		{
			BuildLookup();

			float[] levels = lookupTable[characterClass][stat];
			if (levels == null || levels.Length < level) 
			{
				return 0;
			}
			return levels[level - 1];
		}

		private void BuildLookup()
		{
			if (lookupTable != null) return;

			lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
			foreach (var characterClass in characterClasses)
			{
				var statLookupTable = new Dictionary<Stat, float[]>();
				foreach (var progressionStat in characterClass.stats)
				{
					statLookupTable[progressionStat.stat] = progressionStat.levels;
				}
				lookupTable[characterClass.characterClass] = statLookupTable;
			}


		}
	}

	

	[System.Serializable]
	public class ProgressionCharacterClass
	{
		public CharacterClass characterClass;
		public ProgressionStat[] stats;
	}

	[System.Serializable]
	public class ProgressionStat
	{
		public Stat stat;
		public float[] levels;
	}

	public enum CharacterClass
	{
		Player,
		Grunt,
		Mage,
		Archer,
	}

	public enum Stat
	{
		Health,
		ExperienceReward,

	}
}
