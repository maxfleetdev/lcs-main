using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private GameObject inputManager;
    [SerializeField] private GameObject debugConsole;

    private void OnEnable()
    {
        inputManager.SetActive(true);
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        debugConsole.SetActive(true);
    }
}