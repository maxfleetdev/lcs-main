using System.Collections.Generic;
using UnityEngine;

/*
 * #1 Use overlapshere to find nearby objects with interact layer
 * #2 Send ray to closest object to check if it's visible
 * #3 Yes > Make closest object
 * #4 No > Go to next object
 * #5 If close enough, then interact with object
 */

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private LayerMask interactMask;
    [SerializeField] private float rayDistance = 2f;
    [SerializeField] private float searchRadius = 4f;
    [SerializeField] private Transform originPoint;

    private GameObject closestObject;
    private int maxTargets = 10;

    private void FixedUpdate()
    {
        SearchForObjects();
    }

    private void SearchForObjects()
    {
        Vector3 origin = originPoint.position;
        Collider[] near_objects = new Collider[maxTargets];
        int found_count = Physics.OverlapSphereNonAlloc(origin, searchRadius, near_objects, interactMask);
        if (found_count == 0)
        {
            closestObject = null;
            return;
        }

        float closest_distance = float.MaxValue;
        foreach(Collider collider in near_objects)
        {
            if (collider == null)
            {
                continue;
            }
            Vector3 dir = collider.transform.position;
            float distance = Vector3.Distance(origin, collider.transform.position);
            if (distance < closest_distance)
            {
                closest_distance = distance;
                closestObject = collider.gameObject;
            }

            if (collider != closestObject)
            {
                Debug.DrawRay(origin, dir - origin, Color.red);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(originPoint.position, searchRadius);

        if (closestObject != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(closestObject.transform.position, 0.25f);
            
            Gizmos.color = Color.green;
            Vector3 origin = originPoint.transform.position;
            Vector3 closest = closestObject.transform.position;
            Gizmos.DrawRay(origin, closest - origin);
        }
    }
}