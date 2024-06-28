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
		private Coroutine currentActiveFade;

		private void Awake()
		{
			canvasGroup = GetComponent<CanvasGroup>();
		}

		public void FadeOutImmediate()
		{
			canvasGroup.alpha = 1;
		}

		public Coroutine FadeOut(float time)
		{
			return Fade(1f, time);
		}

		public Coroutine FadeIn(float time)
		{
			return Fade(0f, time);

		}

		public Coroutine Fade(float target, float time)
		{
			if (currentActiveFade != null)
			{
				StopCoroutine(currentActiveFade);
			}
			currentActiveFade = StartCoroutine(FadeRoutine(target, time));
			return currentActiveFade;
		}

		private  IEnumerator FadeRoutine(float target, float time)
		{
			while (!Mathf.Approximately(target, (canvasGroup.alpha)))
			{
				canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
				yield return null;
			}
		}
	}
}
