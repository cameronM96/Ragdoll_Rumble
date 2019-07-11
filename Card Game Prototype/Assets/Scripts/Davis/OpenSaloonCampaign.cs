using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenSaloonCampaign : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene("SaloonCampaign");
    }
}
