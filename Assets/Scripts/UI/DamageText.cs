using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
namespace RPG.UI.DamageText
{
	public class DamageText : MonoBehaviour
	{
		[SerializeField] Text text;
		public void SetValue(float value)
		{
			text.text = value.ToString();
		}
	}
}
