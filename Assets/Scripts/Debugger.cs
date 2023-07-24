using System.Collections;
using UnityEngine;

public static class Debugger
{
    public static void DrawCircle(Vector3 center, float radius, Color color)
    {
        int segments = 360;
        float angle = 0f;
        for (int i = 0; i < segments; i++)
        {
            Vector3 start = center + new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle), 0) * radius;
            angle += 360f / segments;
            Vector3 end = center + new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle), 0) * radius;
            Debug.DrawLine(start, end, color);
        }
    }

    public static void DrawSquare(Vector3 center, float size, Color color)
    {
        Vector3 topLeft = center + new Vector3(-size / 2, size / 2, 0);
        Vector3 topRight = center + new Vector3(size / 2, size / 2, 0);
        Vector3 bottomRight = center + new Vector3(size / 2, -size / 2, 0);
        Vector3 bottomLeft = center + new Vector3(-size / 2, -size / 2, 0);

        Debug.DrawLine(topLeft, topRight, color);
        Debug.DrawLine(topRight, bottomRight, color);
        Debug.DrawLine(bottomRight, bottomLeft, color);
        Debug.DrawLine(bottomLeft, topLeft, color);
    }

}
