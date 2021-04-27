using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AltitudeMeter : MonoBehaviour
{
    public TextMeshProUGUI AltitudeText;
    private Transform playerTR;

    private void Start()
    {
        UpdatePlayerReference();
    }

    private void Update()
    {
        UpdatePlayerReference();
        if (playerTR == null)
        {
            GameObject playerGO = GameManager.GetCurrentPlayer();
            if (playerGO == null)
                return;

            playerTR = playerGO.transform;
        }

        AltitudeText.SetText(GetAltitude() + "M");
    }

    private void UpdatePlayerReference()
    {
        if (playerTR == null)
        {
            GameObject playerGO = GameManager.GetCurrentPlayer();
            if (playerGO == null)
                return;

            playerTR = playerGO.transform;
        }
    }

    public int GetAltitude()
    {
        if (playerTR == null)
            return 0;
        else
            return Mathf.RoundToInt(playerTR.transform.position.y);
    }
}
