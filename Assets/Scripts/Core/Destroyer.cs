using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace RPG.Core
{
	public class Destroyer : MonoBehaviour
	{
		[SerializeField] GameObject targetToDestroy = null;

		public void DestroyObject()
		{
			Destroy(targetToDestroy);
		}
	}
}