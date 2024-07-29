﻿using GameDevTV.Inventories;
using UnityEngine;
using RPG.Stats;
using System;
using System.Collections.Generic;
namespace RPG.Inventories
{
	[CreateAssetMenu(menuName = ("RPG/Inventory/Equipable Item"))]
	public class StatsEquipableItem : EquipableItem, IModifierProvider
	{
		[SerializeField]
		Modifier[] addtiveModifiers;
		[SerializeField]
		Modifier[] percentageModifiers;

		[Serializable]
		struct Modifier 
		{
			public Stat stat;
			public float value;
		}

		public IEnumerable<float> GetAdditiveModifiers(Stat stat)
		{
			foreach (var modifier in addtiveModifiers)
			{
				if (modifier.stat == stat) 
				{
					yield return modifier.value;
				}
			}
		}

		public IEnumerable<float> GetPercentageModifiers(Stat stat)
		{
			foreach (var modifier in percentageModifiers)
			{
				if (modifier.stat == stat)
				{
					yield return modifier.value;
				}
			}
		}
	}
}
