using UnityEngine;
using UnityEngine.Events;

public class InteractEvent : MonoBehaviour, IInteractable
{
    [SerializeField] private UnityEvent interacted;

    public void Interact()
    {
        interacted?.Invoke();
    }
}