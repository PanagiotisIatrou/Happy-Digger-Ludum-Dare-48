using UnityEngine;

public class Shaker : MonoBehaviour
{
	private float shakeTimer = 0;
	private float shakeAmount = 0.05f;
	private float shakeSpeed = 5f;
	private bool shakeOnce = false;
	Vector3 originalPos;
	Vector3 newPos;

	private void Update()
	{
		originalPos = transform.position;
		if (shakeOnce)
		{
			shake();
		}
	}

	public void Shake(float magnitude, float length)
	{
		shakeAmount = magnitude;

		shakeOnce = true;
		shakeTimer = length;
		newPos = transform.position;
	}

	private void shake()
	{
		if (shakeTimer > 0)
		{

			if (Vector3.Distance(newPos, transform.position) <= shakeAmount / 30)
				newPos = originalPos + Random.insideUnitSphere * shakeAmount;

			transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * shakeSpeed);
			shakeTimer -= Time.deltaTime;
		}
		else
		{
			shakeTimer = 0f;
			transform.position = originalPos;
			shakeOnce = false;
		}
	}
}
