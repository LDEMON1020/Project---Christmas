using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItem : MonoBehaviour
{
    public CoinData coinData;
    public AudioClip pickUpSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CoinManager.Instance.AddCoin(coinData.value);
            
            AudioSource.PlayClipAtPoint(pickUpSound, transform.position);

            Destroy(gameObject);
        }
    }
}
