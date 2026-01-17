using UnityEngine;

public class Rock : MonoBehaviour
{
    public float fallSpeed = 5f;

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // ‰æ–ÊŠO‚Éo‚½‚çíœ
        if(transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("“–‚½‚Á‚½");

            GameManager.instance.GameOver();
        }
    }
}
