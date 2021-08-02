using UnityEngine;

public class PreviousPoint
{
    public Vector3 Point;
    public Transform Transform;
    public PreviousPoint(Vector3 point, Transform transform)
    {
        Point = point;
        Transform = transform;
    }
}