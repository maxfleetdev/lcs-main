using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private InputData inputData;
    [SerializeField] private Vector3 interactBox;
    [Space]
    [SerializeField] private string interactTag;
    [SerializeField] private LayerMask interactMask;
    
    private readonly float interactOffset = 0.65f;

    #region Start/Stop

    private void OnEnable()
    {
        inputData.InteractEvent += PlayerInteracted;
    }

    private void OnDisable()
    {
        inputData.InteractEvent -= PlayerInteracted;
    }

    #endregion

    #region Interact Logic

    private void PlayerInteracted()
    {
        Transform obj = ClosestObject();
        if (obj == null) return;

        // Interaction Event
        obj.GetComponent<IInteractable>().Interact();
    }

    private Transform ClosestObject()
    {
        // Setup Box Extents
        Vector3 center = transform.position + transform.forward * interactOffset;
        Vector3 h_size = interactBox / 2f;

        // Find objects within Interact Box
        Collider[] objects = new Collider[4];
        int amount = Physics.OverlapBoxNonAlloc(center, h_size, objects, transform.rotation, interactMask);
        if (amount == 0) return null;
        
        // Main Raycast Loop
        for (int i = 0; i < amount; i++)
        {
            Vector3 col_pos = objects[i].transform.position;
            Vector3 direction = col_pos - transform.position;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == interactTag)
                {
                    return hit.transform;
                }
            }
        }
        return null;
    }

    #endregion

    #region Gizmos

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        DrawInteractBox();
    }

    private void DrawInteractBox()
    {
        Vector3 offset = transform.forward * interactOffset;
        Quaternion rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        Gizmos.matrix = Matrix4x4.TRS(transform.position + offset, rotation, Vector3.one);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, interactBox);
    }
#endif

    #endregion
}