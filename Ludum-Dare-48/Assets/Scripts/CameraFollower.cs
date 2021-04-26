using UnityEngine;

public class CameraFollower : MonoBehaviour
{
	// Singleton
	private static CameraFollower _instance;
	public static CameraFollower Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<CameraFollower>();
			}

			return _instance;
		}
	}

	public Transform targetTR;
	private float positionSmoothTime = 0.05f;

	public static void SetTarget(Transform tr)
	{
		Instance.targetTR = tr;
	}

	private void FixedUpdate()
	{
		if (targetTR == null)
			return;

		Vector3 velocity = Vector3.zero;
		Vector3 newPos = Vector3.SmoothDamp(transform.position, targetTR.position, ref velocity, positionSmoothTime);
		newPos.z = -10;
		transform.position = newPos;
	}
}
