using UnityEngine;

public static class Bezier 
{
	public static Vector3 GetPoint (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		t = Mathf.Clamp01(t);
		float OneMinusT = 1f - t;
		return
			OneMinusT * OneMinusT * OneMinusT * p0 +
			3f * OneMinusT * OneMinusT * t * p1 +
			3f * OneMinusT * t * t * p2 +
			t * t * t * p3;
	}

	public static Vector3 GetFirstDerivative (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		t = Mathf.Clamp01(t);
		float oneMinusT = 1f - t;
		return
			3f * oneMinusT * oneMinusT * (p1 - p0) +
			6f * oneMinusT * t * (p2 - p1) +
			3f * t * t * (p3 - p2);
	}

    // Function to find the closest point on a Bezier curve segment to a target point
    public static float FindClosestT(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 target, int steps = 10)
    {
        float closestT = 0;
        float minDistance = float.MaxValue;

        for (int i = 0; i <= steps; i++)
        {
            float t = i / (float)steps;
            Vector3 point = GetPoint(p0, p1, p2, p3, t);
            float distance = Vector3.Distance(target, point);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestT = t;
            }
        }

        return closestT;
    }
}