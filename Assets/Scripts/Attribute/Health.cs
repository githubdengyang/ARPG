using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attribute
{
	public class Health : MonoBehaviour, ISaveable
	{
		[SerializeField] float health = 100f;

		private void Start()
		{
			health = GetComponent<BaseStats>().GetHealth();
		}

		bool isDead;
		public bool IsDead
		{
			get { return isDead; }
		}

		public void TakeDamage(GameObject instigator, float damage)
		{
			health = Mathf.Max(health - damage, 0);
			if (health == 0)
			{
				Die();
				AwardExperience(instigator);
			}
		}

		private void AwardExperience(GameObject instigator)
		{
			Experience experience = instigator.GetComponent<Experience>();
			if (experience == null) return;
			experience.GainExpeience(GetComponent<BaseStats>().GetExperienceReward());
		}

		public float GetPercentage()
		{
			return 100 * (health / GetComponent<BaseStats>().GetHealth());
		}

		public float GetHealthPoints()
		{
			return health;
		}

		public float GetMaxHealthPoints()
		{
			return GetComponent<BaseStats>().GetHealth();
		}

		private void Die()
		{
			if (isDead) return;
			isDead = true;
			GetComponent<Animator>().SetTrigger("die");
			GetComponent<ActionScheduler>().CancelCurrentAction();
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
