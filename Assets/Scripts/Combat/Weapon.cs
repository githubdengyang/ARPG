using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Combat
{
	[CreateAssetMenu(fileName = "Weapon", menuName = "RPG Project/Make New Weapon", order  = 0)]
	public class Weapon : ScriptableObject
	{
		[SerializeField] GameObject weaponPrefab = null;
		[SerializeField] AnimatorOverrideController animatorOverride = null;
		[SerializeField] public float weaponRange = 2f;
		[SerializeField] public float weaponDamage = 5f;
		[SerializeField] public bool isRightHanded = false;
		[SerializeField] Projectile projectile;

		const string weaponName = "Weapon";

		public void Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
		{
			DestroyOldWeapon(rightHandTransform, leftHandTransform);
			if (weaponPrefab != null)
			{
				Transform transform = GetTransform(rightHandTransform, leftHandTransform);
				GameObject weapon = Instantiate(weaponPrefab, transform);
				weapon.name = weaponName;
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

		public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, float calculatedDamage)
		{
			Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
			projectileInstance.SetTarget(target, calculatedDamage);
		}



		public float GetDamage()
		{
			return weaponDamage;
		}

		public float GetRange()
		{
			return weaponRange;
		}
	}
}
