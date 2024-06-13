using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Transform NodePosition;
    public List<Transform> Connections = new List<Transform>();
}

public class CameraPath : MonoBehaviour
{
    [SerializeField] private List<Node> nodes = new List<Node>();
    [Header("Visuals")]
    [SerializeField] private Color pathColor = Color.green;
    [Space]
    [SerializeField, MinValue(0.01f)] private float nodeSize;
    [SerializeField] private Color nodeColor = Color.blue;

    public List<Node> GetNodes() => nodes;

    private void OnDrawGizmos()
    {
        if (nodes.Count < 2) return;
        int i = 0;
        foreach (var node in nodes)
        {
            if (node.NodePosition == null) continue;

            // Draw Nodes
            Vector3 nodePos = node.NodePosition.position;
            Gizmos.color = nodeColor;
            // Start Node
            if (i == 0)
                Gizmos.color = Color.green;
            // End Node
            if (i == nodes.Count - 1)
                Gizmos.color = Color.red;
            Gizmos.DrawSphere(nodePos, nodeSize);

            // Draw Connections
            foreach (var connectedTransform in node.Connections)
            {
                if (connectedTransform == null) continue;

                Gizmos.color = pathColor;
                Gizmos.DrawLine(nodePos, connectedTransform.position);
            }
            i++;
        }
    }
}