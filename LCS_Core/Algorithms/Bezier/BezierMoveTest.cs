using UnityEngine;

public class BezierMoveTest : MonoBehaviour
{
    [SerializeField] private BezierSpline curve;
    [SerializeField] private Transform target;

    private Vector3 point;
    private void Update()
    {
        point = curve.GetClosestPoint(target.position);
    }

    private void OnDrawGizmos()
    {
        if (point == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(point, 0.2f);
    }
}
