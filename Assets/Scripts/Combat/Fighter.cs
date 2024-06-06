using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Combat
{
	public class Fighter : MonoBehaviour, IAction
	{
		[SerializeField]
		public float weaponRange = 2f;

		[SerializeField]
		public float timeBetweenAttacks = 1f;

		[SerializeField]
		public float weaponDamage = 5f;

		float timeSinceLastAttack = Mathf.Infinity;

		private Health _target;
		
		private void Start()
		{
		}

		private void Update()
		{
			timeSinceLastAttack += Time.deltaTime;


			if (_target == null)
				return;
			if (_target.IsDead)
				return;
			bool isInRange = Vector3.Distance(this.transform.position, _target.transform.position) < weaponRange;
			if (isInRange)
			{
				GetComponent<Mover>().Cancel();
				AttackBehaviour();
				
			}
			else
			{
				GetComponent<Mover>().MoveTo(_target.transform.position);
			}
		}

		public bool CanAttack(GameObject target) 
		{
			if (target == null || target.transform.GetComponent<Health>() == null || target.transform.GetComponent<Health>().IsDead)
				return false;
			return true;
		}

		private void AttackBehaviour()
		{
			transform.LookAt(_target.transform.position);
			if (timeSinceLastAttack >= timeBetweenAttacks)
			{
				SetAttack();
				timeSinceLastAttack = 0;
			}

		}

		private void SetAttack()
		{
			GetComponent<Animator>().ResetTrigger("stopAttack");
			GetComponent<Animator>().SetTrigger("attack");
		}

		public void Attack(GameObject target)
		{
			GetComponent<ActionScheduler>().StartAction(this);
			_target = target.GetComponent<Health>();
		}

		public void Cancel()
		{
			SetStopAttack();
			_target = null;
		}

		private void SetStopAttack()
		{
			GetComponent<Animator>().ResetTrigger("attack");
			GetComponent<Animator>().SetTrigger("stopAttack");
		}

		private void Hit() 
		{
			if (_target == null) return;
			_target.TakeDamage(weaponDamage);
		}
	}
}
