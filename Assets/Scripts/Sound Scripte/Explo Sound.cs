using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploSound : MonoBehaviour
{
    public AudioClip explosionSound;

    void Start()
    {
        AudioSource.PlayClipAtPoint(explosionSound, transform.position);
    }
}
