using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Core
{
	public class Health : MonoBehaviour, ISaveable
	{
		[SerializeField] float health = 100f;

		bool isDead;
		public bool IsDead
		{
			get { return isDead; }
		}

		public void TakeDamage(float damage)
		{
			health = Mathf.Max(health - damage, 0);
			Debug.Log(health);
			Die();
		}

		private void Die()
		{
			if (isDead) return;
			if (health == 0)
			{
				isDead = true;
				GetComponent<Animator>().SetTrigger("die");
				GetComponent<ActionScheduler>().CancelCurrentAction();
			}
		}

		public object CaptureState()
		{
			return health;
		}

		public void RestoreState(object state)
		{
			health = (float)state;
			if (health == 0)
			{
				Die();
			}
		}
	}
}
