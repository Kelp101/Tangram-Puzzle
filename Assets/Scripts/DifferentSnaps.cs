using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DifferentSnaps : MonoBehaviour
{
    public GameObject[] snapPoints;
    private float snapSensitivity = 0.2f;

    private AudioSource audioSource;
    private AudioClip popSound;

    void Start()
    {
        popSound = Resources.Load<AudioClip>("Audio/Pop");
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    public void ApplySnap()
    {
        for (int i = 0; i < snapPoints.Length; i++)
        {
            if (Vector3.Distance(snapPoints[i].transform.position, transform.position) < snapSensitivity)
            {
                transform.position = new Vector3(snapPoints[i].transform.position.x, snapPoints[i].transform.position.y, snapPoints[i].transform.position.z - 0.1f);

                if (audioSource != null && popSound != null)
                {
                    audioSource.PlayOneShot(popSound);
                }
            }
        }
    }
}