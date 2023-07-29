using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Transform posA;
    [SerializeField] private Transform posB;
    [SerializeField] float speed;

    private Vector2 targetPos;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetPos = posA.position;
        if (Vector2.Distance(transform.position, posB.position) < 1f)
            targetPos = posB.position;
        else
            spriteRenderer.flipX = true;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, posA.position) < 1f) targetPos = posB.position;

        if (Vector2.Distance(transform.position, posB.position) < 1f) targetPos = posA.position;

        if(spriteRenderer.flipX && Vector2.Distance(transform.position, posA.position) < 1f || !spriteRenderer.flipX && Vector2.Distance(transform.position, posB.position) < 1f)
            spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
            player.Die();
    }
}
