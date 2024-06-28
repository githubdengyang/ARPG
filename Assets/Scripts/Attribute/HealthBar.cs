using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attribute
{
	public class HealthBar : MonoBehaviour
	{
		[SerializeField] Health healthComponet = null;
		[SerializeField] RectTransform foreground = null;
		[SerializeField] Canvas rootCanvas = null;

		private void Awake()
		{

		}

		private void Update()
		{
			if (Mathf.Approximately(healthComponet.GetFraction(), 0)
			|| (Mathf.Approximately(healthComponet.GetFraction(), 1)))
			{
				rootCanvas.enabled = false;
				return;
			}
			rootCanvas.enabled = true;
			foreground.localScale = new Vector3(healthComponet.GetFraction(), 1f, 1f);
		}
	}
}
