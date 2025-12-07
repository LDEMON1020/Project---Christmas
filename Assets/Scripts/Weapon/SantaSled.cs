using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaSled : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 3f;

    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy"))
            return;

        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}


