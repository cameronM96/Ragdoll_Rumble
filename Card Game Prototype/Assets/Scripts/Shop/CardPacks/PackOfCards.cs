using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Pack")]
public class PackOfCards : ScriptableObject
{
    public int iD;
    public int packSize;
    public string packName;
    public Sprite packImage;
    public int coinCost;
    public int gemCost;

    public bool guaranteedSpecificCards;
    public List<Card> gSpecificCards;

    public bool randomSpecificCards;
    public int guaranteedFromSet;
    public string setName;
    public List<Card> rSpecificCards;

    public bool guaranteedRandom;
    public int numberOfRares;
    public int numberOfUncommons;
    public int numberOfCommons;

    public bool randomCard;
    public int numberOfRandomCards;
}
