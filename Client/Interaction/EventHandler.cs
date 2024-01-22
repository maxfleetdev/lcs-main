using UnityEngine;
using UnityEngine.Events;

public class EventHandler : MonoBehaviour
{
    [Header("Type of Interaction")]
    [SerializeField] private EventType eventType;

    [Header("Event on Interaction")]
    public UnityEvent OnInteracted;

    public void PlayerInteracted()
    {
        OnInteracted?.Invoke();
    }
}