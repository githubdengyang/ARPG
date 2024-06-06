using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Control
{
	public class PatrolPath : MonoBehaviour
	{
		const float waypointGizmoRadius = 0.3f;
		void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			for (int i = 0; i < transform.childCount; i++)
			{
				int nextIndex = GetNextPoint(i);
				Gizmos.DrawSphere(transform.GetChild(i).position, waypointGizmoRadius);
				Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(nextIndex).position);
			}
		}

		public int GetNextPoint(int i)
		{
			return (i + 1) % transform.childCount;
		}
	}
}