using UnityEngine;

public class IcicleDrop : MonoBehaviour
{
    public trapObject trapobj;
    public int Damage = 15;
    public PlayerController player;
    private Rigidbody2D rb;
    public int fallSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(trapobj.isTrapActive == true)
        {
            rb.gravityScale = 2 * fallSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            player.TakeDamage(Damage);
            Destroy(gameObject);
        }
        else if(other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
