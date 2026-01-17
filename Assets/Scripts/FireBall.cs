using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private float speed = 6f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    public void Init(Vector2 dir)
    {
        rb.linearVelocity = dir.normalized * speed;
    }

    private void Update()
    {
        if(transform.position.x < -12f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.GameOver();
        }
    }

    public void Break()
    {
        Destroy (gameObject);
    }
}
