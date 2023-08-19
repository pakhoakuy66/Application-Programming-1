using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDog : MonoBehaviour
{
    [SerializeField]
    private Transform PosA;

    [SerializeField]
    private Transform PosB;

    [SerializeField]
    private float speedEnemy;
    private Vector2 targetPos;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private GameObject player;
    private Transform playerPos;
    private Vector2 currentPos;

    [SerializeField]
    private float distance;
    private Animator enemyAnim;

    private bool isMovingRight = true; // Biến để theo dõi hướng di chuyển của dơi
    private bool isChasingPlayer = false; // Biến để theo dõi trạng thái dơi đuổi theo người chơi

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetPos = PosA.position;
        if (Vector2.Distance(transform.position, PosB.position) < 1f)
            targetPos = PosB.position;
        else
            spriteRenderer.flipX = true;
        playerPos = player.GetComponent<Transform>();
        currentPos = GetComponent<Transform>().position;
        enemyAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, playerPos.position) < distance)
        {
            if (!isChasingPlayer)
            {
                isChasingPlayer = true;
                Flip();
            }

            transform.position = Vector2.MoveTowards(
                transform.position,
                playerPos.position,
                speedEnemy * Time.deltaTime
            );

            if (transform.position.x < playerPos.position.x && isMovingRight)
            {
                isMovingRight = false;
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
            else if (transform.position.x > playerPos.position.x && !isMovingRight)
            {
                isMovingRight = true;
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, PosA.position) < 1f)
                targetPos = PosB.position;

            if (Vector2.Distance(transform.position, PosB.position) < 1f)
                targetPos = PosA.position;

            if (
                spriteRenderer.flipX && Vector2.Distance(transform.position, PosA.position) < 1f
                || !spriteRenderer.flipX && Vector2.Distance(transform.position, PosB.position) < 1f
            )
                spriteRenderer.flipX = !spriteRenderer.flipX;
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPos,
                speedEnemy * Time.deltaTime
            );
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
            player.Die();
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= 1;
        transform.localScale = scale;
    }
}
