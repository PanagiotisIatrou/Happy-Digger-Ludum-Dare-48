using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AltitudeMeter : MonoBehaviour
{
    public TextMeshProUGUI AltitudeText;
    private Transform playerTR;

    private void Update()
    {
        if (playerTR == null)
        {
            GameObject playerGO = GameManager.GetCurrentPlayer();
            if (playerGO == null)
                return;

            playerTR = playerGO.transform;
        }

        AltitudeText.SetText(GetAltitude() + "M");
    }

    public int GetAltitude()
    {
        return Mathf.RoundToInt(playerTR.transform.position.y);
    }
}
