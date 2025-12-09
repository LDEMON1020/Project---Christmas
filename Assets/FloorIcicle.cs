using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorIcicle : MonoBehaviour
{
    public int Damage = 5;
    public PlayerController player;
    // Start is called before the first frame update

   void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.TakeDamage(Damage);
        }
    }
}
