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
    public event Action OnTriggerCalled;
    public event Action OnExitCalled;

    #region Triggers

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == objectTag)
        {
            OnTriggerCalled?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == objectTag && callOnExit)
        {
            OnExitCalled?.Invoke();
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