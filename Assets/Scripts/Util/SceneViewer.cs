using UnityEngine;

public class SceneViewer : MonoBehaviour
{
	public float moveSpeed = 5.0f;  // ����ƶ��ٶ�
	public float lookSpeed = 2.0f;  // ����ӽ���ת�ٶ�

	void Update()
	{
		// �����λ���ƶ�����
		Vector3 moveDirection = Vector3.zero;

		// ���ݰ�����������ƶ�����
		if (Input.GetKey(KeyCode.W))
			moveDirection += transform.forward;
		if (Input.GetKey(KeyCode.S))
			moveDirection -= transform.forward;
		if (Input.GetKey(KeyCode.A))
			moveDirection -= transform.right;
		if (Input.GetKey(KeyCode.D))
			moveDirection += transform.right;

		// �����ƶ�������ٶ��ƶ����λ��
		transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;

		// ����ӽǿ���
		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = Input.GetAxis("Mouse Y");

		// ��������ƶ���������ӽ�
		transform.eulerAngles += new Vector3(-mouseY * lookSpeed, mouseX * lookSpeed, 0.0f);
	}
}
