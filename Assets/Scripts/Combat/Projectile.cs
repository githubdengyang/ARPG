using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Combat
{
	public class Projectile : MonoBehaviour
	{
		[SerializeField] float speed = 1f;
		[SerializeField] Health target = null;
		float damage = 0;
		[SerializeField] bool isHoming = false;
		[SerializeField] GameObject hitEffect = null;
		[SerializeField] float maxLifeTime = 10f;
		[SerializeField] GameObject[] destroyOnHit = null;
		[SerializeField] float lifeAfterImpact = 0f;

		private void Start()
		{
			transform.LookAt(GetAimLocation());
		}

		void Update()
		{
			if (target == null) return;
			if (isHoming && !target.GetComponent<Health>().IsDead)
			{
				transform.LookAt(GetAimLocation());
			}
			transform.Translate(Vector3.forward * speed * Time.deltaTime);
		}

		public void SetTarget(Health target, float damage)
		{
			this.target = target;
			this.damage = damage;
			Destroy(gameObject, maxLifeTime);
		}

		private Vector3 GetAimLocation()
		{
			CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
			if (targetCapsule == null)
			{
				return target.transform.position;
			}
			return target.transform.position + Vector3.up * targetCapsule.height / 2;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.GetComponent<Health>() != target) return;
			if (other.GetComponent<Health>().IsDead) return;
			if (hitEffect != null)
			{
				Instantiate(hitEffect, GetAimLocation(), transform.rotation);
			}
			speed = 0;
			target.TakeDamage(damage);
			Destroy(gameObject, lifeAfterImpact);

			foreach (GameObject toDestroy in destroyOnHit)
			{
				Destroy(toDestroy);
			}
		}
	}
}
