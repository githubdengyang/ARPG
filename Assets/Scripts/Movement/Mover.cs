using RPG.Combat;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;

namespace RPG.Movement
{
	public class Mover : MonoBehaviour,IAction,ISaveable
	{
		private NavMeshAgent _nav;
		private Health health;
		[SerializeField] private float maxSpeed = 6.0f;
		private void Awake()
		{
			_nav = GetComponent<NavMeshAgent>();
			health = GetComponent<Health>();
		}

		private void Update()
		{
			_nav.enabled = !health.IsDead;
			UpdateAnimator();
		}

		private void UpdateAnimator()
		{
			Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
			Vector3 localVelocity = transform.InverseTransformDirection(velocity);
			float speed = localVelocity.z;
			GetComponent<Animator>().SetFloat("forwardSpeed", speed);
		}

		public void StartMoveAction(Vector3 destination, float speedFraction) 
		{
			GetComponent<ActionScheduler>().StartAction(this);
			MoveTo(destination, speedFraction);
		}

		public void MoveTo(Vector3 pos, float speedFraction)
		{
			_nav.isStopped = false;
			_nav.destination = pos;
			_nav.speed = maxSpeed * Mathf.Clamp01(speedFraction);
		}

		public void Cancel()
		{
			_nav.isStopped = true;
		}

		[System.Serializable]
		struct MoverSaveData
		{
			public SerializableVector3 position;
			public SerializableVector3 rotation;
		}

		public object CaptureState()
		{
			MoverSaveData data = new MoverSaveData();
			data.position = new SerializableVector3(transform.position);
			data.rotation = new SerializableVector3(transform.eulerAngles);

			return data;
		}

		public void RestoreState(object state)
		{
			MoverSaveData data = (MoverSaveData)state;
			_nav.enabled = false;
			
			transform.position = ((SerializableVector3)data.position).ToVector();
			transform.eulerAngles = ((SerializableVector3)data.position).ToVector();

			_nav.enabled = true;
		}
	}
}