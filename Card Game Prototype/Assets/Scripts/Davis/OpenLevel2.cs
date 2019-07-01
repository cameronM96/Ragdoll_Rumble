using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenLevel2 : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene("CampaignLevel2");
    }
}
