using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBAT : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private Transform playerPos;
    private Vector2 currentPos;

    [SerializeField]
    private float distance;

    [SerializeField]
    private float speedEnemy;
    private Animator enemyAnim;

    private bool isMovingRight = true; // Biến để theo dõi hướng di chuyển của dơi
    private bool isChasingPlayer = false; // Biến để theo dõi trạng thái dơi đuổi theo người chơi

    void Start()
    {
        playerPos = player.GetComponent<Transform>();
        currentPos = GetComponent<Transform>().position;
        enemyAnim = GetComponent<Animator>();
    }

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
            enemyAnim.SetBool("IsFlying", true);

            // Xác định hướng di chuyển của dơi khi đuổi theo người chơi
            if (transform.position.x < playerPos.position.x && isMovingRight)
            {
                isMovingRight = false;
                Flip();
            }
            else if (transform.position.x > playerPos.position.x && !isMovingRight)
            {
                isMovingRight = true;
                Flip();
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, currentPos) <= 0)
            {
                enemyAnim.SetBool("IsFlying", false);
            }
            else
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    currentPos,
                    speedEnemy * Time.deltaTime
                );
                enemyAnim.SetBool("IsFlying", true);
                if (isChasingPlayer)
                {
                    isChasingPlayer = false;
                    Flip();
                }
            }
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out var player))
            player.Die();
    }
}
