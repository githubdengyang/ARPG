using GameDevTV.Inventories;
using RPG.Attribute;
using RPG.Stats;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
	[CreateAssetMenu(fileName = "Weapon", menuName = "RPG Project/Make New Weapon", order  = 0)]
	public class WeaponConfig : EquipableItem, IModifierProvider
	{
		[SerializeField] Weapon weaponPrefab = null;
		[SerializeField] AnimatorOverrideController animatorOverride = null;
		[SerializeField] public float weaponRange = 2f;
		[SerializeField] public float weaponDamage = 5f;
		[SerializeField] public bool isRightHanded = false;
		[SerializeField] Projectile projectile;
		[SerializeField] float percentageBonus = 0;

		const string weaponName = "Weapon";
		public Weapon Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
		{
			DestroyOldWeapon(rightHandTransform, leftHandTransform);

			Weapon weapon = null; ;

			if (weaponPrefab != null)
			{
				Transform transform = GetTransform(rightHandTransform, leftHandTransform);
				weapon = Instantiate(weaponPrefab, transform);
				weapon.gameObject.name = weaponName;
			}
			var animatorOverrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

			if (animatorOverride != null)
			{
				animator.runtimeAnimatorController = animatorOverride;
			}
			else if(animatorOverrideController != null)
			{
				animator.runtimeAnimatorController = animatorOverrideController.runtimeAnimatorController;
			}

			return weapon;
		}

		private void DestroyOldWeapon(Transform rightHandTransform, Transform leftHandTransform)
		{
			Transform oldWeapon = rightHandTransform.Find(weaponName);
			if (oldWeapon == null)
			{
				oldWeapon = leftHandTransform.Find(weaponName);
			}

			if (oldWeapon == null) return;
			oldWeapon.name = "DESTROYING";
			Destroy(oldWeapon.gameObject);
		}

		private Transform GetTransform( Transform rightHandTransform, Transform leftHandTransform)
		{
			if (isRightHanded)
			{
				return rightHandTransform;
			}
			else
			{
				return leftHandTransform;
			}
		}

		public bool HasProjectile()
		{
			return projectile != null;
		}

		public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target,GameObject instigator, float calculatedDamage)
		{
			Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
			projectileInstance.SetTarget(target, instigator, calculatedDamage);
		}

		public float GetDamage()
		{
			return weaponDamage;
		}

		public float GetPercentageBonus() 
		{
			return percentageBonus;
		}

		public float GetRange()
		{
			return weaponRange;
		}

		public IEnumerable<float> GetAdditiveModifiers(Stat stat)
		{
			if (stat == Stat.Damage)
			{
				yield return weaponDamage;
			}
		}

		public IEnumerable<float> GetPercentageModifiers(Stat stat)
		{
			if (stat == Stat.Damage)
			{
				yield return weaponRange;
			}
		}
	}
}
