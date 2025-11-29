using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GingerCookie : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 3f;
    public int Damage = 20;
    public float attackRate = 0.5f;
    private Transform target;
    public float traceRange = 5f;

    private Vector2 direction;

    public PlayerController player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Start()
    {
        float facing = player.transform.localScale.x;
        target = GetNearestEnemy();

        if (facing < 0)
            direction = Vector2.left;
        else
            direction = Vector2.right;

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (target != null)
        {
            float dist = Vector2.Distance(transform.position, target.position);

            if (dist <= traceRange)
            {
                TraceEnemy();
                return;
            }
        }
      
            transform.Translate(direction * speed * Time.deltaTime);
        
    }

    void TraceEnemy()
    {
        // 타겟이 제거되었다면 새로 찾기
        if (target == null)
        {
            target = GetNearestEnemy();
        }

        // 타겟 방향으로 이동
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Enemy가 아닌 경우 무시하고 벽 관통
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
            Debug.Log("충돌: " + other.name);
        }

        if(other.CompareTag("Enemy"))
        {
           
              other.GetComponent<EnemyController>()?.TakeDamage(Damage);
               other.GetComponent<ReconEnemyController>()?.TakeDamage(Damage);
               other.GetComponent<RangedEnemy>()?.TakeDamage(Damage);
            Destroy(gameObject);
        }
    }

    Transform GetNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
            return null;

        Transform nearest = null;
        float nearestDist = Mathf.Infinity;

        foreach (var e in enemies)
        {
            float dist = Vector2.Distance(transform.position, e.transform.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearest = e.transform;
            }
        }

        return nearest;
    }

    //사용 방법
    //적 오브젝트에게 "Enemy" 태그 달아주기
    //적 오브젝트의 Collider 2D 달아주기 
    //이 스크립트를 사용할 프리팹에게 RigidBody2D 달아주고 Gravity Scale을 0으로 해주기
    // 프리팹의 Collider 2D를 달아주고 IsTrigger 체크하기
}
