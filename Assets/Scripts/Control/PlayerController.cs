using RPG.Attribute;
using RPG.Movement;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using RPG.Combat;

namespace RPG.Control
{
	public class PlayerController : MonoBehaviour
	{
		private Health health;
		[SerializeField] float maxNavMeshProjectionDistance = 1f;
		[System.Serializable]
		struct CursorMapping
		{
			public CursorType type;
			public Vector2 hotspot;
			public Texture2D texture;
		}

		[SerializeField] CursorMapping[] cursorMappings = null;
		public float rotationSpeed = 5.0f;
		public float rotationAmount = 10.0f; // 每次按键旋转的角度
		[SerializeField] Transform camare = null;
		[SerializeField] float raycastRadius = 1f;

		bool isDraggingUI = false;

		void Awake()
		{
			health = GetComponent<Health>();
		}

		void Update()
		{
			if (Input.GetKey(KeyCode.Q))
			{
				RotateCamera(-rotationAmount); // 逆时针旋转
			}
			else if (Input.GetKey(KeyCode.E))
			{
				RotateCamera(rotationAmount); // 顺时针旋转
			}

			if (InteractWithUI()) 
			{
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

		void RotateCamera(float angle)
		{
			if (camare == null) return;
			camare.Rotate(Vector3.up, angle * rotationSpeed * Time.deltaTime, Space.World);
			// 这里可以根据需要调整旋转的空间和速度
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
			RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);
			Array.Sort(hits, (hit1, hit2) => hit1.distance.CompareTo(hit2.distance));
			return hits;
		}

		private bool InteractWithUI()
		{
			if (Input.GetMouseButtonUp(0))
			{
				isDraggingUI = false;
			}

			if (EventSystem.current.IsPointerOverGameObject())
			{
				if (Input.GetMouseButton(0))
				{
					isDraggingUI = true;
				}
				SetCursor(CursorType.UI);
				return true;
			}

			if (isDraggingUI)
			{ 
				return true; 
			}

			return false;
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
				if (!GetComponent<Mover>().CanMoveTo(target)) 
				{
					return false;
				}
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

			return true;
		}

	

		private static Ray GetMouseRay()
		{
			return Camera.main.ScreenPointToRay(Input.mousePosition);
		}
	}
}