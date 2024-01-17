using UnityEngine;

/*
 * #1 Use overlapshere to find nearby objects with interact layer
 * #2 Send ray to closest object to check if it's visible
 * #3 Yes > Make closest object
 * #4 No > Go to next object
 * #5 If close enough, then interact with object
 * 
 * Kinda done, needs refining
 */

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private LayerMask interactMask;
    [SerializeField] private float rayDistance = 2f;
    [SerializeField] private float searchRadius = 4f;
    [SerializeField] private Transform originPoint;

    private GameObject closestObject;
    private InputManager inputManager;

    private Vector3 vec1;
    private Vector3 vec2;

    private int maxTargets = 10;

    #region Runtime

    private void Start()
    {
        inputManager = InstanceFinder.Input_Manager();
        inputManager.OnInteract += PlayerInteract;
    }

    private void OnDisable()
    {
        inputManager.OnInteract -= PlayerInteract;
    }

    private void FixedUpdate()
    {
        SearchForObjects();
    }

    #endregion

    #region Search For Objects
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

    private void PlayerInteract()
    {
        // point where overlap sphere will be
        Vector3 detect_point = originPoint.position + transform.forward;
        vec1 = detect_point + transform.up * 0.75f;
        vec2 = detect_point + -transform.up * 1.25f;
        Collider[] found = Physics.OverlapCapsule(vec1, vec2, 0.5f, interactMask);

        // null check
        if (found.Length <= 0)
        {
            DebugSystem.Log("Non-found...", LogType.Debug);
            return;
        }

        // send event to eventhandler
        EventHandler handler = found[0].transform.GetComponent<EventHandler>();
        if (handler != null)
        {
            handler.PlayerInteracted();
            DebugSystem.Log("Interacting...", LogType.Debug);
        }
    }

    #endregion

    #region Gizmos

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

        Gizmos.color = Color.yellow;
        DrawWireCapsule(vec1, vec2, 0.5f);
    }

    public static void DrawWireCapsule(Vector3 p1, Vector3 p2, float radius)
    {
#if UNITY_EDITOR
        // Special case when both points are in the same position
        if (p1 == p2)
        {
            // DrawWireSphere works only in gizmo methods
            Gizmos.DrawWireSphere(p1, radius);
            return;
        }
        using (new UnityEditor.Handles.DrawingScope(Gizmos.color, Gizmos.matrix))
        {
            Quaternion p1Rotation = Quaternion.LookRotation(p1 - p2);
            Quaternion p2Rotation = Quaternion.LookRotation(p2 - p1);
            // Check if capsule direction is collinear to Vector.up
            float c = Vector3.Dot((p1 - p2).normalized, Vector3.up);
            if (c == 1f || c == -1f)
            {
                // Fix rotation
                p2Rotation = Quaternion.Euler(p2Rotation.eulerAngles.x, p2Rotation.eulerAngles.y + 180f, p2Rotation.eulerAngles.z);
            }
            // First side
            UnityEditor.Handles.DrawWireArc(p1, p1Rotation * Vector3.left, p1Rotation * Vector3.down, 180f, radius);
            UnityEditor.Handles.DrawWireArc(p1, p1Rotation * Vector3.up, p1Rotation * Vector3.left, 180f, radius);
            UnityEditor.Handles.DrawWireDisc(p1, (p2 - p1).normalized, radius);
            // Second side
            UnityEditor.Handles.DrawWireArc(p2, p2Rotation * Vector3.left, p2Rotation * Vector3.down, 180f, radius);
            UnityEditor.Handles.DrawWireArc(p2, p2Rotation * Vector3.up, p2Rotation * Vector3.left, 180f, radius);
            UnityEditor.Handles.DrawWireDisc(p2, (p1 - p2).normalized, radius);
            // Lines
            UnityEditor.Handles.DrawLine(p1 + p1Rotation * Vector3.down * radius, p2 + p2Rotation * Vector3.down * radius);
            UnityEditor.Handles.DrawLine(p1 + p1Rotation * Vector3.left * radius, p2 + p2Rotation * Vector3.right * radius);
            UnityEditor.Handles.DrawLine(p1 + p1Rotation * Vector3.up * radius, p2 + p2Rotation * Vector3.up * radius);
            UnityEditor.Handles.DrawLine(p1 + p1Rotation * Vector3.right * radius, p2 + p2Rotation * Vector3.left * radius);
        }
#endif
    }

    #endregion
}