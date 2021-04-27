using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

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

    public AudioClip ExplosionSound;
    public AudioClip ClickSound;
    public AudioClip CoinPickupSound;
    public AudioClip PowerupSound;

    public PostProcessVolume PP;
    public AltitudeMeter AltMeter;

    public GameObject InventoryGO;

    public CanvasGroup StartMenuGroup;
    public CanvasGroup InterfaceGroup;

    private GameObject playerGO;

    private bool gameStarted = false;

    public static GameObject GetCurrentPlayer()
    {
        return Instance.playerGO;
    }

    private void SpawnPlayer()
    {
        playerGO = Instantiate(PlayerPrefab, new Vector3(4.5f, 1f, -1f), Quaternion.identity);
        playerGO.name = "Player";
        CameraFollower.SetTarget(playerGO.transform);
    }

    public static void RespawnPlayer()
    {
        Instance.StartCoroutine(Instance.IERespawnPlayer());
    }

    private IEnumerator IERespawnPlayer()
    {
        yield return new WaitForSeconds(1.5f);
        AudioSource.PlayClipAtPoint(PowerupSound, new Vector3(4.5f, 1f, -1f));
        SpawnPlayer();
    }

    public static void StartGame()
    {
        if (Instance.gameStarted)
            return;

        Instance.gameStarted = true;
        Instance.StartCoroutine(Instance.IEStartGame());
    }

    private IEnumerator IEStartGame()
    {
        while (StartMenuGroup.alpha > 0f && InterfaceGroup.alpha < 1f)
        {
            StartMenuGroup.alpha -= 1f / 60f;
            InterfaceGroup.alpha += 1f / 60f;
            yield return null;
        }

        SpawnPlayer();
    }
}
