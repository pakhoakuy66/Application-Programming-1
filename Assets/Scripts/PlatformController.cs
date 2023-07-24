using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private Transform posA, posB;
    [SerializeField] private int speed;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private BoxCollider2D objectCollider;

    private Vector2 targetPos;
    private float colliderHeight = .2f;
    private BoxCollider2D triggerCollider;

    private void Awake()
    {
        targetPos = posB.position;
        posA.parent = posB.parent = null;
        triggerCollider = gameObject.AddComponent<BoxCollider2D>();
        triggerCollider.isTrigger = true;
        triggerCollider.size = new Vector2(objectCollider.size.x, colliderHeight);
        triggerCollider.offset = new Vector2(0, objectCollider.size.y / 2 - colliderHeight / 2);
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, posA.position) < 1f) targetPos = posB.position;

        if (Vector2.Distance(transform.position, posB.position) < 1f) targetPos = posA.position;
    }

    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
            collision.gameObject.transform.parent = transform;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
            collision.gameObject.transform.parent = null;
    }
}
