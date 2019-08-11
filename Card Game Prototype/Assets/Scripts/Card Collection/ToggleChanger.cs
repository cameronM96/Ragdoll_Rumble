using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleChanger : MonoBehaviour
{
    public CollectionManager cm;
    public int value;

    public void ToggleChanged(Toggle toggle)
    {
        cm.ToggleChange(value, toggle);
    }
}
