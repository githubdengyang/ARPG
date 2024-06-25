using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Attribute;
using RPG.Stats;
using GameDevTV.Utils;

namespace RPG.Combat
{
	public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
	{
		[SerializeField] public float timeBetweenAttacks = 1f;
		[SerializeField] Transform rightHandTransform = null;
		[SerializeField] Transform leftHandTransform = null;
		[SerializeField] Weapon defaultWeapon;

		float timeSinceLastAttack = Mathf.Infinity;
		private Health target;
		LazyValue<Weapon> currentWeapon ;

		private void Awake()
		{
			currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
		}

		private void Start()
		{
			currentWeapon.ForceInit();
		}

		private Weapon SetupDefaultWeapon()
		{
			AttachWeapon(defaultWeapon);
			return defaultWeapon;
		}

		public Health GetTarget()
		{
			return target;
		}

		public void EquipWeapon(Weapon weapon)
		{
			currentWeapon.value = weapon;
			AttachWeapon(weapon);
		}

		private void AttachWeapon(Weapon weapon)
		{
			Animator animator = GetComponent<Animator>();
			weapon.Spawn(rightHandTransform, leftHandTransform, animator);
		}

		private void Update()
		{
			timeSinceLastAttack += Time.deltaTime;


			if (target == null)
				return;
			if (target.IsDead)
				return;
			bool isInRange = Vector3.Distance(this.transform.position, target.transform.position) < currentWeapon.value.GetRange();
			if (isInRange)
			{
				GetComponent<Mover>().Cancel();
				AttackBehaviour();
				
			}
			else
			{
				GetComponent<Mover>().MoveTo(target.transform.position, 1f);
			}
		}

		public bool CanAttack(GameObject target) 
		{
			if (target == null
				|| target == this.gameObject
				|| target.transform.GetComponent<Health>() == null
				|| target.transform.GetComponent<Health>().IsDead)
				return false;
			return true;
		}

		private void AttackBehaviour()
		{
			transform.LookAt(target.transform.position);
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
			this.target = target.GetComponent<Health>();
		}

		public void Cancel()
		{
			SetStopAttack();
			target = null;
			GetComponent<Mover>().Cancel();	
		}

		private void SetStopAttack()
		{
			GetComponent<Animator>().ResetTrigger("attack");
			GetComponent<Animator>().SetTrigger("stopAttack");
		}

		// Animation Event
		private void Hit() 
		{
			if (target == null) return;

			float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
			if (currentWeapon.value.HasProjectile())
			{
				currentWeapon.value.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
			}
			else
			{
				target.TakeDamage(gameObject, damage);
			}
		}

		// Animation Event
		private void Shoot()
		{
			Hit();
		}

		public object CaptureState()
		{
			return currentWeapon.value.name;
		}

		public void RestoreState(object state)
		{
			string weaponName = (string)state;
			Weapon weapon = Resources.Load<Weapon>(weaponName);
			EquipWeapon(weapon);
		}

		public IEnumerable<float> GetAdditiveModifiers(Stat stat)
		{
			if (stat == Stat.Damage)
			{
				yield return currentWeapon.value.GetDamage();
			}
		}

		public IEnumerable<float> GetPercentageModifiers(Stat stat)
		{
			if (stat == Stat.Damage)
			{
				yield return currentWeapon.value.GetPercentageBonus();
			}
		}
	}
}
