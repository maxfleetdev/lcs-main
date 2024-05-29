using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewGameDataSave : MonoBehaviour, ISubmitHandler
{
    public event Action OnSubmit;

    void ISubmitHandler.OnSubmit(BaseEventData eventData)
    {
        OnSubmit?.Invoke();
    }
}