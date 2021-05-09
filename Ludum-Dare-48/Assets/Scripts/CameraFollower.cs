using UnityEngine;
using EZCameraShake;

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

	public CameraShaker shaker;

	private Transform targetTR;
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
		newPos.y = Mathf.Clamp(newPos.y, Mathf.NegativeInfinity, 5f);
		transform.position = newPos;
	}

    private void Update()
    {
		GameObject playerGO = GameManager.GetCurrentPlayer();
		if (playerGO != null)
			shaker.RestPositionOffset = transform.position;
	}
}
