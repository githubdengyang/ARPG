using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
	public class ExperienceDisplay : MonoBehaviour
	{
		Experience experience = null;
		Text text = null;

		private void Awake()
		{
			experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
			text = GetComponent<Text>();
		}

		private void Update()
		{
			text.text = String.Format("{0:0}", experience.GetPoints());
		}
	}
}
