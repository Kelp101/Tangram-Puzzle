using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CompletePuzzle : MonoBehaviour
{
    public DifferentSnaps[] tangramPieces;
    private bool tangramCompleted = false;
    public GameObject winTextMessage;

    void Update()
    {
        if (!tangramCompleted && AllTangramPiecesPlaced())
        {
            tangramCompleted = true;
            // Debug.Log("Tangram Completed");
            OnTangramComplete();
        }
    }

    bool AllTangramPiecesPlaced()
    {
        foreach (DifferentSnaps piece in tangramPieces)
        {
            bool snapped = false;

            foreach (GameObject snapPoint in piece.snapPoints)
            {
                if (Vector3.Distance(piece.transform.position, snapPoint.transform.position) < 0.2f)
                {
                    snapped = true;
                    break;
                }
            }

            if (!snapped)
            {
                return false;
            }
        }

        return true;
    }

    void OnTangramComplete()
    {
        // Debug.Log("All Pieces Placed! You win the tangram!");
        if (winTextMessage != null)
        {
            winTextMessage.SetActive(true);
            // Debug.Log("Win text pops up");
        }
        /*
        else
        {
            Debug.LogError("Wintext not in inspector");
        }
        */
    }
}