using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Attribute;

namespace RPG.Combat
{
	public class WeaponPickup : MonoBehaviour, IRaycastable
	{
		[SerializeField] WeaponConfig weapon = null;
		[SerializeField] float respawnTime = 5f;
		[SerializeField] float healthToRestore = 0;


		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.tag == "Player")
			{
				Pickup(other.gameObject);
			}
		}

		private void Pickup(GameObject subject)
		{
			if(weapon != null)
			{
				Fighter fighter = subject.GetComponent<Fighter>();
				fighter.EquipWeapon(weapon);
			}
			if (healthToRestore > 0)
			{
				subject.GetComponent<Health>().Heal(healthToRestore);
			}
			StartCoroutine(Respawn(respawnTime));
		}

		public bool HandleRaycast(PlayerController callingController)
		{
			if (Input.GetMouseButtonDown(0)) 
			{
				Pickup(callingController.gameObject);
			}

			return true;
		}

		public CursorType GetCursorType() 
		{
			return CursorType.Pickup;
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
