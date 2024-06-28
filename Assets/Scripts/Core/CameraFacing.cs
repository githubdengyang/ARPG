using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace RPG.Core
{
	public class CameraFacing : MonoBehaviour
	{
		[SerializeField] private Camera _camera;
		private void Awake()
		{
			_camera = Camera.main;
		}
		private void LateUpdate()
		{
			transform.forward = _camera.transform.forward;
		}

	}
}