using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarDamage : MonoBehaviour
{
    public int damage = 3;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            ReconEnemyController recon = collision.GetComponent<ReconEnemyController>();
            if (recon != null)
            {
                recon.TakeDamage(damage);
            }
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            RangedEnemy ranged = collision.GetComponent<RangedEnemy>();
            if (ranged != null)
            {
                ranged.TakeDamage(damage);
            }
            GrenadierEnemy Greandier = collision.GetComponent<GrenadierEnemy>();
            if (Greandier != null)
            {
                Greandier.TakeDamage(damage);
            }
        }

        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }    
    }
}