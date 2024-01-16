using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Interaction manager is in-charge of interacting within the world.
    It simply sends an event to the interacted object. This works the same
    for both first and third person.

    Uses OverlapSphere to cast rays to nearby objects (rank in distance).
 */

public class InteractionManager : MonoBehaviour
{
    // Serializable //
    [SerializeField] private float searchRange = 4f;
    [SerializeField] private float rayLength = 2f;
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private Transform originPoint;

    // Class-only //
    protected private List<GameObject> nearbyObjects = new List<GameObject>();
    
    private InputManager inputManager;
    private bool canInteract = false;
    private GameObject objectFound = null;

    #region Runtime/Startup

    private void Start()
    {
        inputManager = InstanceFinder.Input_Manager();
        nearbyObjects.Clear();

        inputManager.OnInteract += OnInteracted;
    }

    private void OnDisable()
    {
        inputManager.OnInteract -= OnInteracted;
    }

    private void FixedUpdate()
    {
        FindNearbyObjects();
    }

    #endregion

    private void FindNearbyObjects()
    {
        Collider[] nearby_objects = Physics.OverlapSphere(originPoint.position, searchRange, interactLayer); // initial search
        if (nearby_objects.Length == 0)
        {
            objectFound = null;
            return;
        }

        Vector3 origin = originPoint.position;
        foreach (Collider collider in nearby_objects)
        {
            // Raycast to the found objects //
            Vector3 obj_pos = collider.transform.position;
            GameObject found_object = collider.gameObject;
            if (Physics.Raycast(origin, obj_pos - origin, rayLength, interactLayer))
            {
                if (!nearbyObjects.Contains(found_object))
                {
                    nearbyObjects.Add(found_object);
                }
            }

            // If not detected remove from nearby object //
            else if (nearbyObjects.Contains(found_object))
            {
                nearbyObjects.Remove(found_object);
            }
        }

        if (nearbyObjects.Count >= 1)
        {
            MathsLib.Sort(nearbyObjects, originPoint.position);       // sort list [closest>furthest]
            objectFound = nearbyObjects[0];
            canInteract = true;
        }

        else
        {
            objectFound = null;
            canInteract = false;
        }
    }


    #region InputEvents

    private void OnInteracted()
    {

    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        // Draw Search Range //
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(originPoint.position, searchRange);

        // Draw Ray to Object //
        if (canInteract)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(originPoint.position, objectFound.transform.position);
        }
    }
}