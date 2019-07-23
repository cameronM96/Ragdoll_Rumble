using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumTypes;

public class ShopManagerv2 : MonoBehaviour
{
    public Text coins;
    public Text gems;
    public Text playerName;

    public Player playerInfo;

    public int RareDefaultCoinCost;
    public int RareDefaultGemCost;
    public int UncommonDefaultCoinCost;
    public int UncommonDefaultGemCost;
    public int CommonDefaultCoinCost;
    public int CommonDefaultGemCost;

    // Update is called once per frame
    void Update()
    {
        if (playerInfo == null)
            playerInfo = GameObject.FindGameObjectWithTag("PlayerProfile").GetComponent<Player>();

        playerName.text = playerInfo.playerName;
        coins.text = "" + playerInfo.coins;
        gems.text = "" + playerInfo.gems;
    }

    public int ReturnDefaultCosts(bool premium, Card card)
    {
        int defaultCost = -1;

        if (premium)
        {
            switch (card.rarity)
            {
                case Rarity.None:
                    break;
                case Rarity.Common:
                    defaultCost = CommonDefaultGemCost;
                    break;
                case Rarity.Uncommon:
                    defaultCost = UncommonDefaultGemCost;
                    break;
                case Rarity.Rare:
                    defaultCost = RareDefaultGemCost;
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (card.rarity)
            {
                case Rarity.None:
                    break;
                case Rarity.Common:
                    defaultCost = CommonDefaultCoinCost;
                    break;
                case Rarity.Uncommon:
                    defaultCost = UncommonDefaultCoinCost;
                    break;
                case Rarity.Rare:
                    defaultCost = RareDefaultCoinCost;
                    break;
                default:
                    break;
            }
        }

        //Debug.Log(defaultCost);
        return defaultCost;
    }
}
