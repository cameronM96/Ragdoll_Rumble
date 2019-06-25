using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeywordInputField : MonoBehaviour
{
    private InputField iField;

    private void Awake()
    {
        iField = GetComponent<InputField>();
    }

    public void Clear()
    {
        iField.text = "";
        DeckBuildingScreen.Instance.collectionBrowserScript.keyword = iField.text;
    }

    public void EnterSubmit()//work out how to do this on mobile without needing to press the enter key, perhaps an auto search as you type (would that lag too much?)
    {
        DeckBuildingScreen.Instance.collectionBrowserScript.keyword = iField.text;
    }
}
