using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleWindows : MonoBehaviour
{
    public List<GameObject> windows;

    public void ToggleUI(GameObject myWindow)
    {
        foreach (GameObject window in windows)
        {
            if (window != myWindow)
            {
                window.SetActive(false);
            }
            else
                window.SetActive(true);
        }
    }
}
