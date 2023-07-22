using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField]private Transform posA, posB;
    [SerializeField]private int speed;
    private Vector2 targetPos;

    private void Awake()
    {
        targetPos = posB.position;
        posA.parent = posB.parent = null;
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
}
