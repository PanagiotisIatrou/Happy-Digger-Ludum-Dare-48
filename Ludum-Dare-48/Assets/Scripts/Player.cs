using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Player : MonoBehaviour
{
    private FuelManager fuelManager;
    private bool died = false;

    private void Start()
    {
        fuelManager = GetComponent<FuelManager>();
    }

    private void Update()
    {
        if (died)
            return;

        if (fuelManager.GetFuel() <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        died = true;
        GameObject explosion = Instantiate(GameManager.Instance.ExplosionPrefab, transform.position, Quaternion.identity);
        explosion.transform.position += new Vector3(0, 0, -1);
        AudioSource.PlayClipAtPoint(GameManager.Instance.ExplosionSound, transform.position);
        Destroy(gameObject);
        CameraShaker.Instance.ShakeOnce(3f, 2f, 0.25f, 0.25f);
        GameManager.RespawnPlayer();
    }

}
