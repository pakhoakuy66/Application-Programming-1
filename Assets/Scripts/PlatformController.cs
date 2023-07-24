using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private Transform posA, posB;
    [SerializeField] private int speed;
    [SerializeField] private Transform playerStand;
    [SerializeField] private Collider2D objectCollider;
    [SerializeField] private Player player;
    [SerializeField] private LayerMask playerMask;

    private Vector2 targetPos;
    private Vector2 castSize;

    private const float castHeight = .1f;

    private void Awake()
    {
        targetPos = posB.position;
        posA.parent = posB.parent = null;
        castSize = new Vector2(objectCollider.bounds.size.x, castHeight);
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, posA.position) < 1f) targetPos = posB.position;

        if (Vector2.Distance(transform.position, posB.position) < 1f) targetPos = posA.position;

        if (Physics2D.BoxCast(playerStand.position, castSize, 0f, Vector2.up, 0f, playerMask))
            player.transform.parent = transform;
        else player.transform.parent = null;
    }

    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.fixedDeltaTime);
    }
}
