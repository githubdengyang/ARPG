using RPG.Core;
using GameDevTV.Saving;
using RPG.Stats;
using UnityEngine;
using GameDevTV.Utils;
using UnityEngine.Events;
using System;

namespace RPG.Attribute
{
	public class Health : MonoBehaviour, ISaveable
	{
		LazyValue<float> healthPoint;
		[SerializeField] float regenerationPercentage = 70;
		bool isDead;
		[SerializeField] TakeDamageEvent takeDamage;
		[SerializeField] UnityEvent onDie;

		[Serializable] public class TakeDamageEvent : UnityEvent<float> 
		{
		
		}

		private void Awake()
		{
			healthPoint = new LazyValue<float>(GetInitialHealth);
		}

		private void Start() 
		{
			healthPoint.ForceInit();
		}


		private float GetInitialHealth() 
		{
			return GetComponent<BaseStats>().GetStat(Stat.Health);
		}

		private void OnEnable()
		{
			GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
		}

		private void OnDisable()
		{
			GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
		}

		public void Heal(float healPoint) 
		{
			healthPoint.value = Mathf.Min(healthPoint.value + healPoint, GetMaxHealthPoints());
		}

		private void RegenerateHealth()
		{
			float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
			healthPoint.value = Mathf.Max(healthPoint.value, regenHealthPoints);
		}

		
		public bool IsDead
		{
			get { return isDead; }
		}

		public void TakeDamage(GameObject instigator, float damage)
		{
			print(gameObject.name + " took damage: " + damage);
			takeDamage.Invoke(damage);
			healthPoint.value = Mathf.Max(healthPoint.value - damage, 0);
			if (healthPoint.value == 0)
			{
				onDie.Invoke();
				Die();
				AwardExperience(instigator);
			}
		}

		private void AwardExperience(GameObject instigator)
		{
			Experience experience = instigator.GetComponent<Experience>();
			if (experience == null) return;
			experience.GainExpeience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
		}

		public float GetPercentage()
		{
			return 100 * healthPoint.value / GetComponent<BaseStats>().GetStat(Stat.Health);
		}

		public float GetFraction()
		{
			return healthPoint.value / GetComponent<BaseStats>().GetStat(Stat.Health);
		}

		public float GetHealthPoints()
		{
			return healthPoint.value;
		}

		public float GetMaxHealthPoints()
		{
			return GetComponent<BaseStats>().GetStat(Stat.Health);
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
			return healthPoint.value;
		}

		public void RestoreState(object state)
		{
			healthPoint.value = (float)state;
			if (healthPoint.value == 0)
			{
				Die();
			}
		}
	}
}
