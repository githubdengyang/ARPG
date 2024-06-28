using RPG.Attribute;
using RPG.Movement;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace RPG.Control
{
	public class PlayerController : MonoBehaviour
	{
		private Health health;
		[SerializeField] float maxNavMeshProjectionDistance = 1f;
		[SerializeField] float maxNavPathLength = 40f;
		[System.Serializable]
		struct CursorMapping
		{
			public CursorType type;
			public Vector2 hotspot;
			public Texture2D texture;
		}

		[SerializeField] CursorMapping[] cursorMappings = null;

		void Awake()
		{
			health = GetComponent<Health>();
		}

		void Update()
		{
			if (InteractWithUI()) 
			{
				SetCursor(CursorType.UI);
				return;
			}
			if (health.IsDead)
			{
				SetCursor(CursorType.None);
				return;
			}

			if (InteractWithComponent())
			{
				return;
			}

			if (InteractWithMovement())
			{
				SetCursor(CursorType.Movement);
				return;
			}

			SetCursor(CursorType.None);
		}


		private bool InteractWithComponent()
		{
			RaycastHit[] hits = RaycastAllSorted();
			foreach (var hit in hits)
			{
				IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
				foreach (var raycastable in raycastables)
				{
					if (raycastable.HandleRaycast(this)) 
					{
						SetCursor(raycastable.GetCursorType());
						return true;
					}
				}
			}
			return false;
		}

		private RaycastHit[] RaycastAllSorted()
		{
			RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
			Array.Sort(hits, (hit1, hit2) => hit1.distance.CompareTo(hit2.distance));
			return hits;
		}

		private bool InteractWithUI()
		{
			return EventSystem.current.IsPointerOverGameObject();
		}

		private void SetCursor(CursorType cursorType)
		{
			CursorMapping cursorMapping = GetCursorMapping(cursorType);
			Cursor.SetCursor(cursorMapping.texture, cursorMapping.hotspot, CursorMode.Auto);
		}

		private CursorMapping GetCursorMapping(CursorType type)
		{
			foreach (var mapping in cursorMappings)
			{
				if (mapping.type == type)
				{
					return mapping;
				}
			}
			return cursorMappings[0];
		}

		private bool InteractWithMovement()
		{
			Ray lastRay = GetMouseRay();

			RaycastHit hit;
			bool hasHit = RaycastNavMesh(out Vector3 target);

			if (hasHit)
			{
				if (Input.GetMouseButton(0))
				{
					this.GetComponent<Mover>().StartMoveAction(target, 1f);
				}
				return true;
			}
			return false;
		}

		private bool RaycastNavMesh(out Vector3 target)
		{
			target = new Vector3();
			RaycastHit hit;
			bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
			if (!hasHit) return false;

			bool hasNavMeshHit = NavMesh.SamplePosition(hit.point, out NavMeshHit navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
			if (!hasNavMeshHit) return false;

			target = navMeshHit.position;

			NavMeshPath path = new NavMeshPath();
			bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
			if (!hasPath) return false;

			if (path.status != NavMeshPathStatus.PathComplete) return false;

			if (GetPathLength(path) > maxNavPathLength) return false;

			return true;
		}

		private float GetPathLength(NavMeshPath path)
		{
			float total = 0;
			if (path.corners.Length < 2) return total;

			for (int i = 0; i < path.corners.Length - 1; i++)
			{
				total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
			}
			return total;
		}

		private static Ray GetMouseRay()
		{
			return Camera.main.ScreenPointToRay(Input.mousePosition);
		}
	}
}