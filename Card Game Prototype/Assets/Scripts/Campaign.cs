using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Campaign : MonoBehaviour
{
    Player player;
    public int[] campaignNumbers = new int[4];
    public Button[] campaignButtons = new Button[4];

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerProfile").GetComponent<Player>();
        for (int i = 0; i < campaignNumbers.Length; ++i)
        {
            if (player.CampaignProgress + 1 < campaignNumbers[i] && campaignButtons[i] != null)
                campaignButtons[i].interactable = false;
        }
    }
}
