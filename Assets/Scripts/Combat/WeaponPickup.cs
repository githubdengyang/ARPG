using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Combat
{
	public class WeaponPickup : MonoBehaviour
	{
		[SerializeField] Weapon weapon = null;
		[SerializeField] float respawnTime = 5f;
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.tag == "Player")
			{
				other.GetComponent<Fighter>().EquipWeapon(weapon);
				StartCoroutine(Respawn(respawnTime));
			}
		}

		private IEnumerator Respawn(float respawnTime)
		{
			ShowPickup(false);
			yield return new WaitForSeconds(respawnTime);
			ShowPickup(true);
		}

		private void ShowPickup(bool shoouldShow)
		{
			this.GetComponent<Collider>().enabled = shoouldShow;
			foreach (Transform child in this.transform)
			{
				child.gameObject.SetActive(shoouldShow);
			}
		}
	}
}
