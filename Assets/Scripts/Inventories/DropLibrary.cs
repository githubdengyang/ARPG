using GameDevTV.Inventories;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using static UnityEditor.Progress;

namespace RPG.Inventories
{
	[CreateAssetMenu(menuName = ("RPG/Inventory/Drop Library"))]
	public class DropLibrary : ScriptableObject
	{
		[SerializeField]
		DropConfig[] potentialDrops;
		[SerializeField] float[] dropChancePercentage;
		[SerializeField] int[] minDrops;
		[SerializeField] int[] maxDrops;


		[System.Serializable]
		class DropConfig 
		{ 
			public InventoryItem item;
			public float[] relativeChance;
			public int[] minNumber;
			public int[] maxNumber;

			public int GetRandomNumber(int level) 
			{ 
				if(!item.IsStackable())
				{
					return 1;
				}
				int min = GetByLevel(minNumber, level);
				int max = GetByLevel(maxNumber, level);
				return Random.Range(min, max + 1);	
			}
		}

		public struct Dropped 
		{
			public InventoryItem item;
			public int number;
		}

		public IEnumerable<Dropped> GetRandomDrops(int level)
		{
			if (!ShouldRandomDrop(level))
			{
				yield break;
			}
			for (int i = 0; i < UnityEngine.Random.Range(GetByLevel(minDrops, level), GetByLevel(maxDrops, level)); i++)
			{
				yield return GetRandomDrop(level);
			}
		}

		bool ShouldRandomDrop(int level)
		{
			return Random.Range(0, 100) < GetByLevel(dropChancePercentage, level); 
		}

		Dropped GetRandomDrop(int level) 
		{
			var item = SelectRandomItem(level);
			Dropped dropped = new Dropped
			{
				item = item.item,
				number = item.GetRandomNumber(level)
			};
			return dropped;
		}

		DropConfig SelectRandomItem(int level) 
		{
			float totalChance = GetTotalChance(level);
			float randomRoll = Random.Range(0, totalChance);
			foreach (var item in potentialDrops)
			{
				if (randomRoll < GetByLevel(item.relativeChance, level))
				{
					return item;
				}
				randomRoll -= GetByLevel(item.relativeChance, level);
			}
			return null;
		}

		float GetTotalChance(int level)
		{
			float total = 0;
			foreach (var drop in potentialDrops)
			{
				total += GetByLevel(drop.relativeChance, level) ;
			}
			return total;
		}

		static T GetByLevel<T>(T[] values, int level)
		{
			if (values.Length == 0) return default;
			if (level > values.Length)
			{
				return values[values.Length - 1];
			}
			if (level <= 0)
			{
				return values[0];
			}
			return values[level - 1];
		}
	}
}
