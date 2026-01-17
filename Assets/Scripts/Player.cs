using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 5.0f;

    [SerializeField] public GameObject punchAttackArea;

    Rigidbody2D rb;
    Animator anim;

    float moveX;

    bool isAttacking = false;

    // 共通スケール
    Vector3 baseScale = new Vector3(3f, 3f, 3f);

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // 避けゲー用の設定
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        // スケール固定
        transform.localScale = baseScale;
    }

    void Update()
    {
        moveX = 0f;

        if (!isAttacking)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                moveX -= 1f;
            else if (Input.GetKey(KeyCode.RightArrow))
                moveX += 1f;
        }

        // 移動アニメーション切り替え
        anim.SetBool("isMoving",moveX != 0f);

        // 向き反転
        if (moveX != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveX) * baseScale.x, baseScale.y, baseScale.z);
        }

        // ==== パンチ入力 ====
        if(!isAttacking && Input.GetKeyDown(KeyCode.Space))
        {
            isAttacking = true;
            anim.SetTrigger("Punch");
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveX * moveSpeed, 0f);
    }

    public void PunchAttackStart()
    {
      punchAttackArea.SetActive(true);
    }

    public void PucnEnd()
    {
        punchAttackArea.SetActive(false);
        isAttacking = false;
    }
}
