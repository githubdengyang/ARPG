using RPG.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attribute
{
	public class EnemyHealthDisplay : MonoBehaviour
	{
		Fighter fighter = null;
		Health health = null;
		Text text = null;

		private void Awake()
		{
			fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
			text = GetComponent<Text>();
		}

		private void Update()
		{
			health = fighter.GetTarget();
			if (health != null)
			{
				text.text = String.Format("{0:0.00}/{1:0.00}", health.GetHealthPoints(), health.GetMaxHealthPoints());
			}
			else
			{
				text.text = "N/A";
			}
		}
	}
}
