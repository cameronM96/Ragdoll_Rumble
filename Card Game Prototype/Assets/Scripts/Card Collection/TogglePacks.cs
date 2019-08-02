using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePacks : MonoBehaviour
{
    public string collectionName;
    public string packName;
    public GameObject collectionWindow;
    public GameObject packWindow;
    public Text buttonText;

    public void ToggleWindows()
    {
        collectionWindow.SetActive(!collectionWindow.activeSelf);
        packWindow.SetActive(!collectionWindow.activeSelf);
        if (collectionWindow.activeSelf)
            buttonText.text = packName;
        else
            buttonText.text = collectionName;
    }
}
