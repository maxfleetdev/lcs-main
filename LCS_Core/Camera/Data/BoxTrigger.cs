using NaughtyAttributes;
using System;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BoxTrigger : MonoBehaviour
{
    [SerializeField, Tag] private string objectTag;
    [SerializeField] 
    [Tooltip("Acts as a secondary trigger (when another trigger is active)")]
    private bool nestedTrigger = false;
    [Space]
    [SerializeField] private bool debug = true;
    [SerializeField, ShowIf("debug")] private Color debugColor;
    
    private BoxCollider boxSize;

    // Trigger Events
    public event Action<bool> OnTriggerCalled;
    public event Action<bool> OnExitCalled;

    #region Triggers

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(objectTag))
        {
            OnTriggerCalled?.Invoke(nestedTrigger);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(objectTag) && nestedTrigger)
        {
            OnExitCalled?.Invoke(true);
        }
    }

    #endregion

    #region Debugging

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (debug)
        {
            boxSize = GetComponent<BoxCollider>();
            Gizmos.color = debugColor;
            // Apply transformations for proper scale, rotation, and position
            Matrix4x4 rot_matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
            Gizmos.matrix = rot_matrix;

            Gizmos.DrawWireCube(boxSize.center, boxSize.size);

            // Reset Gizmos matrix
            Gizmos.matrix = Matrix4x4.identity;

            if (nestedTrigger)
            {
                GUI.color = Color.black;
                Handles.Label(this.transform.position, "NESTED");
            }
        }
    }

#endif
    #endregion
}