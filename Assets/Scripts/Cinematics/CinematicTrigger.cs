using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
		private bool _hasPlayed = false;
		private void OnTriggerEnter(Collider other)
		{
			if (_hasPlayed)
				return;
			if (other.gameObject.tag == "Player")
			{
				GetComponent<PlayableDirector>().Play();
				_hasPlayed = true;

			}
		}
	}
}
