using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
            }

            return _instance;
        }

    }

    public GameObject PlayerPrefab;
    public GameObject ExplosionPrefab;
    public Slider FuelSlider;
    public TextMeshProUGUI MoneyText;

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        GameObject playerGO = Instantiate(PlayerPrefab, new Vector3(2.5f, 1f, -1f), Quaternion.identity);
        CameraFollower.SetTarget(playerGO.transform);
    }

    public static void RespawnPlayer()
    {
        Instance.StartCoroutine(Instance.IERespawnPlayer());
    }

    private IEnumerator IERespawnPlayer()
    {
        yield return new WaitForSeconds(1.5f);
        SpawnPlayer();
    }
}
