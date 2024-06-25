using RPG.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
	public class LevelDisplay : MonoBehaviour
	{
		BaseStats baseStats = null;
		Text text = null;

		private void Awake()
		{
			baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
			text = GetComponent<Text>();
		}

		private void Update()
		{
			text.text = String.Format("{0:0}", baseStats.GetLevel());
		}
	}
}
