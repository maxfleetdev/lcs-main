using NaughtyAttributes;
using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BoxTrigger : MonoBehaviour
{
    [SerializeField, Tag] private string objectTag;
    [SerializeField] private bool callOnExit = false;
    [Space]
    [SerializeField] private bool debug = true;
    [SerializeField, ShowIf("debug")] private Color debugColor;
    
    private Vector3 boxSize;

    // Trigger Events
    public event Action<bool> OnTriggerCalled;
    public event Action<bool> OnExitCalled;

    #region Triggers

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == objectTag)
        {
            OnTriggerCalled?.Invoke(callOnExit);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == objectTag && callOnExit)
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
            boxSize = GetComponent<BoxCollider>().size;
            Gizmos.color = debugColor;
            Gizmos.DrawWireCube(this.transform.position, boxSize);
        }
    }

#endif
    #endregion
}