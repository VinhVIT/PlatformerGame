using UnityEngine;

public class Arrow : BaseProjectTile
{
    [SerializeField] private float gravity;
    [SerializeField] private float travelDistance;
    private float xStartPos;
    private bool isGravityOn;
    private bool hasHitGround;
    protected override void Start()
    {
        base.Start();
        rb.gravityScale = 0.0f;
        isGravityOn = false;

        xStartPos = transform.position.x;
    }
    protected override void Update()
    {
        base.Update();

        if (!hasHitGround)
        {
            // attackDetails.position = transform.position;

            if (isGravityOn)
            {
                float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            hasHitGround = true;
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject, 2f);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //out of range
        if (Mathf.Abs(xStartPos - transform.position.x) >= travelDistance && !isGravityOn)
        {
            isGravityOn = true;
            rb.gravityScale = gravity;
        }
    }
}
