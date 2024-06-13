using UnityEngine;

public static class PathUtils
{
    public static void FindClosestLineSegment(Vector3[] path_points, Vector3 target_position,
                                                out int closest_segment_index, out Vector3 closest_point)
    {
        closest_segment_index = 0;
        closest_point = Vector3.zero;
        float closest_distance = Mathf.Infinity;

        for (int i = 0; i < path_points.Length - 1; i++)
        {
            Vector3 line_start = path_points[i];
            Vector3 line_end = path_points[i + 1];
            Vector3 projected_point = ProjectPointOntoLine(target_position, line_start, line_end);
            float distance = Vector3.Distance(target_position, projected_point);

            if (distance < closest_distance)
            {
                closest_distance = distance;
                closest_segment_index = i;
                closest_point = projected_point;
            }
        }
    }

    public static Vector3 ProjectPointOntoLine(Vector3 point, Vector3 line_start, Vector3 line_end)
    {
        Vector3 line_vec = line_end - line_start;
        Vector3 point_vec = point - line_start;
        float projection_length = Vector3.Dot(point_vec, line_vec) / Vector3.Dot(line_vec, line_vec);
        projection_length = Mathf.Clamp(projection_length, 0, 1);
        return line_start + projection_length * line_vec;
    }

    public static void FindClosestPointOnPath(Vector3[] pathPoints, Vector3 targetPosition, int currentSegmentIndex,
                                            out int closestSegmentIndex, out Vector3 closestPoint, out float progress)
    {
        closestSegmentIndex = currentSegmentIndex; // Start with the current segment
        closestPoint = pathPoints[currentSegmentIndex];
        progress = 0f;
        float closestDistance = float.MaxValue;

        for (int i = Mathf.Max(0, currentSegmentIndex - 1); i <= Mathf.Min(pathPoints.Length - 2, currentSegmentIndex + 1); i++)
        {
            // Consider only the current segment and its immediate neighbors
            Vector3 projectedPoint = ProjectPointOntoLine(targetPosition, pathPoints[i], pathPoints[i + 1]);

            // Calculate the distance between the target position and the projected point on the current segment
            float distance = Vector3.Distance(targetPosition, projectedPoint);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestSegmentIndex = i;
                closestPoint = projectedPoint;
            }
        }

        // Calculate progress along the closest segment (0 to 1)
        progress = Vector3.Dot(closestPoint - pathPoints[closestSegmentIndex], pathPoints[closestSegmentIndex + 1] - pathPoints[closestSegmentIndex]) /
                   (pathPoints[closestSegmentIndex + 1] - pathPoints[closestSegmentIndex]).sqrMagnitude;

        // Clamp progress to ensure the closest point remains within the segment boundaries
        progress = Mathf.Clamp01(progress);

        // Update closestPoint to the clamped position on the segment
        closestPoint = Vector3.Lerp(pathPoints[closestSegmentIndex], pathPoints[closestSegmentIndex + 1], progress);
    }
}