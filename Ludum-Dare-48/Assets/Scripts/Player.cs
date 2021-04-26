using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using UnityEngine.Rendering.PostProcessing;

public class Player : MonoBehaviour
{
    private FuelManager fuelManager;
    private bool died = false;
    private Vignette vignette;
    private float maxVignetteIntensity = 0.4f;

    private void Start()
    {
        fuelManager = GetComponent<FuelManager>();
        GameManager.Instance.PP.profile.TryGetSettings(out vignette);
    }

    private void Update()
    {
        if (died)
            return;

        int altitude = GameManager.Instance.AltMeter.GetAltitude();
        if (altitude >= 0)
        {
            vignette.intensity.Override(0.1f);
        }
        else
        {
            float vignetteIntensity = 0.1f + Mathf.Clamp(((float)Mathf.Abs(altitude) / 200f) * maxVignetteIntensity, 0f, 0.3f);
            vignette.intensity.Override(vignetteIntensity);
        }

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
