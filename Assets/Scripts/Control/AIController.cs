using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Control
{
	public class AIController : MonoBehaviour
	{
		[SerializeField] float chaseDistance = 5f;
		[SerializeField] float suspicionTime = 2;
		[SerializeField] PatrolPath patrolPath;
		[SerializeField] float waypointTolerance = 1f;
		[SerializeField] float waypointDwellTime = 5;
		[SerializeField] float patrolSpeedFraction = 0.3f;

		private Fighter fighter;
		private GameObject player;
		private Health health;
		private Vector3 guardPosition;
		private Mover mover;
		private float timeSinceLastSawPlayer = Mathf.Infinity;
		private float timeSinceArrivedAtWaypoint = Mathf.Infinity;

		private ActionScheduler actionScheduler;
		private int currWaypointIndex = 0;


		private void Start()
		{
			fighter = GetComponent<Fighter>();
			player = GameObject.FindGameObjectWithTag("Player");
			health = GetComponent<Health>();
			mover = GetComponent<Mover>();
			
			actionScheduler = GetComponent<ActionScheduler>();
			guardPosition = transform.position;
		}

		private void Update()
		{
			if (health.IsDead)
			{
				return;
			}
			if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
			{
				
				AttackBehaviour();
			}
			else if (timeSinceLastSawPlayer < suspicionTime)
			{
				SuspicionBehaviour();
			}
			else
			{
				PatrolBehaviour();
			}

			UpdateTimer();
		}

		private void UpdateTimer()
		{
			timeSinceLastSawPlayer += Time.deltaTime;
			timeSinceArrivedAtWaypoint += Time.deltaTime;
		}

		private void PatrolBehaviour()
		{
			Vector3 nextPosition = guardPosition;
			if (patrolPath != null) 
			{
				if (AtWayPoint()) 
				{
					CycleWaypoint();
					timeSinceArrivedAtWaypoint = 0;
				}
				nextPosition = GetCurrentWayPoint();
			}

			if (timeSinceArrivedAtWaypoint > waypointDwellTime)
			{
				mover.StartMoveAction(nextPosition, patrolSpeedFraction);
			}
		}

		private bool AtWayPoint()
		{
			float distanceToWayPoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
			return distanceToWayPoint < waypointTolerance;
		}

		private void CycleWaypoint()
		{
			currWaypointIndex = patrolPath.GetNextPoint(currWaypointIndex);
		}

		private Vector3 GetCurrentWayPoint()
		{
			return patrolPath.transform.GetChild(currWaypointIndex).position;
		}

		private void SuspicionBehaviour()
		{
			Debug.Log("timeSinceLastSawPlayer:" + timeSinceLastSawPlayer);
			actionScheduler.CancelCurrentAction();
		}

		private void AttackBehaviour()
		{
			timeSinceLastSawPlayer = 0;
			fighter.Attack(player);
		}

		private bool InAttackRangeOfPlayer()
		{
			return Vector3.Distance(player.transform.position, this.transform.position)<chaseDistance;
		}

		void OnDrawGizmos()
		{
			// Draw a yellow sphere at the transform's position
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, chaseDistance);
		}
	}
}