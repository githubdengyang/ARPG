using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using GameDevTV.Utils;
namespace RPG.Attribute
{
	public class Health : MonoBehaviour, ISaveable
	{
		LazyValue<float> healthPoint;
		[SerializeField] float regenerationPercentage = 70;
		bool isDead;

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
			healthPoint.value = Mathf.Max(healthPoint.value - damage, 0);
			if (healthPoint.value == 0)
			{
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
