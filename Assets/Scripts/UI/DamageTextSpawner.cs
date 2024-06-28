using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
	public class DamageTextSpawner : MonoBehaviour
	{
		[SerializeField] private DamageText damageTextPrefab;
		private void Start()
		{
		}

		public void Spawn(float value) 
		{
			DamageText damageText = Instantiate(damageTextPrefab, transform);
			damageText.SetValue(value);
		}

	}
}
