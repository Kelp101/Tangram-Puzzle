using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    private GameObject objSelected = null;
    private Vector3 offset;
    private float zDistance;

    public GameObject[] snapPoints;
    private float snapSensitivity = 0.2f;

    private AudioSource audioSource;
    private AudioClip clickSound;
    private AudioClip popSound;
    private AudioClip bumpSound;

    void Start()
    {
        clickSound = Resources.Load<AudioClip>("Audio/Click");
        popSound = Resources.Load<AudioClip>("Audio/Pop");
        bumpSound = Resources.Load<AudioClip>("Audio/Bump");

        audioSource = gameObject.AddComponent<AudioSource>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckHitObject(false);
        }
        if (Input.GetMouseButton(0) && objSelected != null)
        {
            DragObject();
        }
        if (Input.GetMouseButtonUp(0) && objSelected != null)
        {
            DropObject();
        }
        if (Input.GetMouseButtonDown(1))
        {
            CheckHitObject(true);
        }
    }

    void CheckHitObject(bool isRotating)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                objSelected = hit.transform.gameObject;

                if (isRotating)
                {
                    RotateObject();
                    objSelected = null;
                    return;
                }

                if (audioSource != null && clickSound != null)
                {
                    audioSource.PlayOneShot(clickSound);
                }

                // make sure z pos doesn't change by storing it
                zDistance = Camera.main.WorldToScreenPoint(objSelected.transform.position).z;

                // offset to maintain relative positioning
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDistance));
                offset = objSelected.transform.position - worldPosition;
            }
        }
    }

    void DragObject()
    {
        if (objSelected != null)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDistance)) + offset;

            // limit boarder incase someone drags a piece out the map by accident
            float minX = -3.5f;
            float maxX = 4.5f;
            float minY = -0.5f;
            float maxY = 3f;

            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

            objSelected.transform.position = newPosition;
        }
    }

    void DropObject()
    {
        DifferentSnaps differentSnaps = objSelected.GetComponent<DifferentSnaps>();

        if (differentSnaps == null)
        {
            for (int i = 0; i < snapPoints.Length; i++)
            {
                if (Vector3.Distance(snapPoints[i].transform.position, objSelected.transform.position) < snapSensitivity)
                {
                    objSelected.transform.position = new Vector3(snapPoints[i].transform.position.x, snapPoints[i].transform.position.y, snapPoints[i].transform.position.z - 0.1f);

                    /* Debug.Log("Snap successful! Playing Pop sound..."); didnt work because snapping a piece was checked in DifferentSnaps.cs (REMINDER)
                    
                    if (audioSource != null && popSound != null)
                    {
                        audioSource.PlayOneShot(popSound);
                    }
                    */
                }
            }
        }
        else
        {
            differentSnaps.ApplySnap();
        }
        objSelected = null;
    }

    void RotateObject()
    {
        if(objSelected != null)
        {
            objSelected.transform.rotation *= Quaternion.Euler(0, 45f, 0);
        }

        if (audioSource != null && popSound != null)
        {
            audioSource.PlayOneShot(bumpSound);
        }
    }
}