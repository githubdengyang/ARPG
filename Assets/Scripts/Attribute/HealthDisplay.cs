using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attribute
{
	public class HealthDisplay : MonoBehaviour
	{
		[SerializeField] Health health = null;
		Text text = null;

		private void Awake()
		{
			health = GameObject.FindWithTag("Player").GetComponent<Health>();
			text = GetComponent<Text>();
		}

		private void Update()
		{
			text.text = String.Format("{0:0.00}%", health.GetPercentage());
		}
	}
}
