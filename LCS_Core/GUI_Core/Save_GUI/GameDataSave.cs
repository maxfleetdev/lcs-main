using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameDataSave : MonoBehaviour, ISelectHandler, ISubmitHandler
{
    [SerializeField] private TextMeshProUGUI textMesh;
    
    private GameData gameData;

    public event Action<int> OnSelected;
    public event Action OnSubmit;

    public void OnSelect(BaseEventData eventData)
    {
        OnSelected?.Invoke(gameData.DataIndex);
    }

    void ISubmitHandler.OnSubmit(BaseEventData eventData)
    {
        OnSubmit?.Invoke();
    }

    public void Construct(GameData data)
    {
        if (data == null)
            return;

        gameData = data;
        textMesh.text = data.SaveLocation;
    }
}