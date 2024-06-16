using UnityEditor;
using UnityEngine;

/// <summary>
/// Used for objects which require to be bound in world space
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class CameraBounds : MonoBehaviour
{
    private BoxCollider boxCollider;

    #region Start

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    #endregion

    #region Debug

    #if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider>();

        // Draw Bounds
        Color box_color = new Color(1f, 0.6f, 0f, 1f);
        Gizmos.color = box_color;

        // Apply transformations for proper scale, rotation, and position
        Matrix4x4 rot_matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Gizmos.matrix = rot_matrix;

        Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);

        // Reset Gizmos matrix
        Gizmos.matrix = Matrix4x4.identity;

        // Draw Min/Max Points (in local space)
        Vector3 min = boxCollider.center - boxCollider.size / 2;
        Vector3 max = boxCollider.center + boxCollider.size / 2;

        // Handle Label
        GUI.color = Color.black;
        Handles.Label(transform.position, "BOUNDS");

        // Draw handles at the corners for visualization
        DrawCornerHandle(rot_matrix, min);
        DrawCornerHandle(rot_matrix, max);
    }

    private void DrawCornerHandle(Matrix4x4 rotation_matrix, Vector3 local_pos)
    {
        Vector3 world_pos = rotation_matrix.MultiplyPoint3x4(local_pos);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(world_pos, 0.05f);
    }

#endif

#endregion

    public Vector3 GetCenter() => boxCollider.center;
    public Vector3 GetSize() => boxCollider.size;
}