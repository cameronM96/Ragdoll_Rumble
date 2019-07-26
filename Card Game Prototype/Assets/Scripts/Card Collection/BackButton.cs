using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public CollectionManager cM;

    private void Start()
    {
        cM = transform.root.GetComponent<CollectionManager>();
    }

    public void Back()
    {
        if (cM.creatingDeck)
            cM.ToggleDeckCreation();
        else
            SceneManager.LoadScene("StephenTestMaps");
    }
}
