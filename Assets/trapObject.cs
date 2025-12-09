using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapObject : MonoBehaviour
{
    public bool isTrapActive = false;
    void Start()
    {
        isTrapActive = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isTrapActive = true;
            Destroy(gameObject);
        }

    }
}
