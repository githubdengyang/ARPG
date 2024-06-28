using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using Unity.Jobs;
using RPG.Saving;
using RPG.Control;

namespace RPG.SceneManagement
{
	public class Portal : MonoBehaviour
	{
		enum DestinationIdentifier
		{
			A, B, C, D
		}

		[SerializeField] int sceneToLoad = -1;

		[SerializeField] Transform spawnPoint;

		[SerializeField] DestinationIdentifier destination;

		[SerializeField] float fadeOutTime = 1f;
		[SerializeField] float fadeInTime = 2f;
		[SerializeField] float fadeWaitTime = 0.5f;


		GameObject player;

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.tag == "Player")
			{
				player = other.gameObject;
				StartCoroutine(Transition());
			}
		}

		private IEnumerator Transition() 
		{
			DontDestroyOnLoad(gameObject);
			Fader fader = FindObjectOfType<Fader>();

			//remove control from player
			PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
			playerController.enabled = false;

			yield return fader.FadeOut(fadeOutTime);

			SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
			wrapper.Save();

			yield return SceneManager.LoadSceneAsync(sceneToLoad);

			//remove control from player
			PlayerController newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
			newPlayerController.enabled = false;

			wrapper.Load();

			Portal otherPortal = GetOtherPortal();
			UpdatePlayer(otherPortal);

			yield return new WaitForSeconds(fadeWaitTime);
			wrapper.Save();

			//restore control from player
			newPlayerController.enabled = true;

			fader.FadeIn(fadeInTime);

			Destroy(gameObject);

		}


		private void UpdatePlayer(Portal otherPortal)
		{
			GameObject player = GameObject.FindWithTag("Player");
			player.GetComponent<NavMeshAgent>().enabled = false;
			player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
			player.GetComponent<NavMeshAgent>().enabled = true;
			player.transform.rotation = otherPortal.spawnPoint.rotation;
		}
	
		private Portal GetOtherPortal()
		{
			foreach (Portal portal in FindObjectsOfType<Portal>())
			{
				if (portal == this) continue;
				if (portal.destination != destination) continue;
				return portal;
			}
			return null;
		}
	}
}
