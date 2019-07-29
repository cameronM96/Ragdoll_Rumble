using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileLoader : MonoBehaviour
{
    public GameObject playerProfile;
    private GameObject checker;

    private void Start()
    {
        checker = GameObject.FindGameObjectWithTag("PlayerProfile");
        if (checker == null)
        {
            checker = Instantiate(playerProfile);
        }
    }
}
