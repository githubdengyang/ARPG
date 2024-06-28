using UnityEngine;

public class SceneViewer : MonoBehaviour
{
	public float moveSpeed = 5.0f;  // 相机移动速度
	public float lookSpeed = 2.0f;  // 相机视角旋转速度

	void Update()
	{
		// 相机的位置移动控制
		Vector3 moveDirection = Vector3.zero;

		// 根据按键输入调整移动方向
		if (Input.GetKey(KeyCode.W))
			moveDirection += transform.forward;
		if (Input.GetKey(KeyCode.S))
			moveDirection -= transform.forward;
		if (Input.GetKey(KeyCode.A))
			moveDirection -= transform.right;
		if (Input.GetKey(KeyCode.D))
			moveDirection += transform.right;

		// 根据移动方向和速度移动相机位置
		transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;

		// 相机视角控制
		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = Input.GetAxis("Mouse Y");

		// 根据鼠标移动调整相机视角
		transform.eulerAngles += new Vector3(-mouseY * lookSpeed, mouseX * lookSpeed, 0.0f);
	}
}
