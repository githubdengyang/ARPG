﻿using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attribute;
using RPG.Control;

namespace RPG.Combat
{
	[RequireComponent(typeof(Health))]
	public class CombatTarget : MonoBehaviour, IRaycastable
	{
		public bool HandleRaycast(PlayerController callingController)
		{
			if (!enabled) return false;
			if (!callingController.GetComponent<Fighter>().CanAttack(gameObject))
			{
				return false;
			}

			if (Input.GetMouseButtonDown(0))
			{
				callingController.GetComponent<Fighter>().Attack(gameObject);
			}

			return true;
		}

		public CursorType GetCursorType()
		{
			return CursorType.Combat;
		}
	}
}
