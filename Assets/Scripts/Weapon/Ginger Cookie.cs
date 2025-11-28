using UnityEngine;

public class GingerCookie : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 3f;

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
        // 플레이어가 바라보는 방향 결정
        float facing = player.transform.localScale.x;

        if (facing < 0)
            direction = Vector2.left;
        else
            direction = Vector2.right;

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
            transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Enemy가 아닌 경우 무시하고 벽 관통
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
            Debug.Log("충돌: " + other.name);
        }
    }

    //사용 방법
    //적 오브젝트에게 "Enemy" 태그 달아주기
    //적 오브젝트의 Collider 2D 달아주기 
    //이 스크립트를 사용할 프리팹에게 RigidBody2D 달아주고 Gravity Scale을 0으로 해주기
    // 프리팹의 Collider 2D를 달아주고 IsTrigger 체크하기
}
