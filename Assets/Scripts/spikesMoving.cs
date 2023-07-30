using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class spikesMoving : MonoBehaviour
{
    public float speed;
    Vector3 targetPos;

    public GameObject ways;
    public Transform[] waysPoint;
    int pointIndex;
    int pointCount;
    int derection = 1;

    public float waitduration;
    int speedMultiplier = 1;

    private void Awake()
    {
        waysPoint = new Transform[ways.transform.childCount];
        for(int i = 0; i < ways.gameObject.transform.childCount; i++)
        {
            waysPoint[i] = ways.transform.GetChild(i).gameObject.transform;
        }
    }

    private void Start()
    {
        pointCount = waysPoint.Length;
        pointIndex = 1;
        targetPos = waysPoint[pointIndex].transform.position;
    }

    private void Update()
    {
        var step = speedMultiplier * speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

        if(transform.position == targetPos)
        {
            NextPoints();
        }
    }

    void NextPoints()
    {
        if(pointIndex == pointCount - 1) //Arrived last point
        {
            derection = -1;
        }

        if(pointIndex == 0) //Arrived first point
        {
            derection = 1;
        }

        pointIndex += derection;
        targetPos = waysPoint[pointIndex].transform.position;
        StartCoroutine(waitNextPoint());
    }

    IEnumerator waitNextPoint()
    {
        speedMultiplier = 0;
        yield return new WaitForSeconds(waitduration);
        speedMultiplier = 1;
    }
}
