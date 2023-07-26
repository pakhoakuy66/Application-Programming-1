using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] float speed;

    private Vector2 targetPos;

    private void Awake()
    {
        targetPos = pointA.position;
    }

    void Update()
    {
        if(Vector2.Distance(transform.position, pointA.position) < 1f)
        {
            Flip();
            targetPos = pointB.position;
        }

        if (Vector2.Distance(transform.position, pointB.position) < 1f)
        {
            Flip();
            targetPos = pointA.position;
        }
    }

    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.fixedDeltaTime);
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
