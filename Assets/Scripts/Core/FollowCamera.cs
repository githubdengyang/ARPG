using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace RPG.Core
{
	public class FollowCamera : MonoBehaviour
	{
		[SerializeField] Transform target;
		private void LateUpdate()
		{
			this.transform.position = target.transform.position;
		}
	}
}