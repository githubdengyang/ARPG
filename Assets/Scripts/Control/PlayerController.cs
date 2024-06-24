using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attribute;

namespace RPG.Control
{
	public class PlayerController : MonoBehaviour
	{
		private Health health;

		// Start is called before the first frame update
		void Start()
		{
			health = GetComponent<Health>();
		}

		// Update is called once per frame
		void Update()
		{
			if (health.IsDead)
			{
				return;
			}
			if (InteractWithCombat())
			{
				return;
			}

			if (InteractWithMovement())
			{
				return;
			}
		}

		private bool InteractWithCombat()
		{
			Ray lastRay = GetMouseRay();

			RaycastHit[] hits = Physics.RaycastAll(lastRay);
			foreach (var hit in hits)
			{
				if (!GetComponent<Fighter>().CanAttack(hit.transform.gameObject))
					continue;

				if (Input.GetMouseButtonDown(0))
				{
					this.GetComponent<Fighter>().Attack(hit.transform.gameObject);
				}
				return true;
			}

			return false;
		}

		private bool InteractWithMovement()
		{
			Ray lastRay = GetMouseRay();

			RaycastHit hit;
			bool hasHit = Physics.Raycast(lastRay, out hit);

			if (hasHit)
			{
				if (Input.GetMouseButton(0))
				{
					this.GetComponent<Mover>().StartMoveAction(hit.point, 1f);
				}
				return true;
			}
			return false;
		}

		private static Ray GetMouseRay()
		{
			return Camera.main.ScreenPointToRay(Input.mousePosition);
		}
	}
}