using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attribute;
using System;
using UnityEngine.Events;

namespace RPG.Combat
{
	public class Weapon : MonoBehaviour
	{
		[SerializeField] UnityEvent onHit;
		public void OnHit()
		{
			onHit.Invoke();
		}
	}
}
