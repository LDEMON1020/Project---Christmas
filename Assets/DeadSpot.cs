using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadSpot : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.Die();
            }
        }
    }

    }
