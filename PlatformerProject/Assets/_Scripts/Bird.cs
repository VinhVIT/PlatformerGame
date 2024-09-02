using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] private float flyAwaySpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float minJumpInterval = 1f;
    [SerializeField] private float maxJumpInterval = 3f;

    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D col;
    private bool isFlyingAway = false;
    private float jumpTimer;
    private int jumpDirection = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();

    }

    private void Update()
    {
        if (!isFlyingAway)
        {
            JumpRandomly();
        }
    }

    private void JumpRandomly()
    {

        if (jumpTimer <= 0)
        {
            jumpDirection = Random.Range(0, 2) == 0 ? 1 : -1;
            rb.velocity = new Vector2(jumpDirection * jumpForce, jumpForce);
            jumpTimer = Random.Range(minJumpInterval, maxJumpInterval);

            if (jumpDirection > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        jumpTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isFlyingAway)
        {
            isFlyingAway = true;
            anim.SetBool("fly", true);
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            FlyAway(direction);
            Destroy(gameObject, 5f);
        }
    }

    private void FlyAway(Vector2 direction)
    {
        Vector2 flyDirection = new Vector2(direction.x, 1).normalized;
        col.enabled = false;
        rb.velocity = flyDirection * flyAwaySpeed;
        rb.gravityScale = 0;
        if (flyDirection.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (flyDirection.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
