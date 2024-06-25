using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using Unity.Jobs;

namespace RPG.SceneManagement
{
	public class Fader : MonoBehaviour
	{
		private CanvasGroup canvasGroup;


		private void Awake()
		{
			canvasGroup = GetComponent<CanvasGroup>();
		}

		public void FadeOutImmediate()
		{
			canvasGroup.alpha = 1;
		}

		public IEnumerator FadeOut(float time)
		{
			Debug.Log("Fading out");
			while (true) 
			{
				canvasGroup.alpha += Time.deltaTime / time;
				if (canvasGroup.alpha >= 1)
				{
					canvasGroup.alpha = 1;
					yield break;
				}
				yield return null;
			}
		}

		public IEnumerator FadeIn(float time)
		{
			Debug.Log("Fading in");
			while (true)
			{
				canvasGroup.alpha -= Time.deltaTime / time;
				if (canvasGroup.alpha <= 0)
				{
					canvasGroup.alpha = 0;
					yield break;
				}
				yield return null;
			}
		}
	}
}
