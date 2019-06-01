﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : ScriptableObject
{
    private enum CardType { Weapon, Armour, Ability, Behaviour, Environmental };
    [SerializeField] private CardType currentCardType = CardType.Weapon;
    private enum PlayableSlot { Head, Chest, Hand, Feet };
    [SerializeField] private PlayableSlot playableSlots = PlayableSlot.Head;

    public string cardName;
    public string description;

    public Sprite artwork;
    public Sprite background;

    public int attack;
    public int armour;
    public int hP;
    public float speed;
    public float atkSpeed;

    public TriggerCondition triggerCondition;

    public Ability2 ability;

    public GameObject[] item;

    public int getCardType ()
    {
        int cardNumb = -1;

        switch (currentCardType)
        {
            case CardType.Weapon:
                cardNumb = 0;
                break;
            case CardType.Armour:
                cardNumb = 1;
                break;
            case CardType.Ability:
                cardNumb = 2;
                break;
            case CardType.Behaviour:
                cardNumb = 3;
                break;
            case CardType.Environmental:
                cardNumb = 4;
                break;
            default:
                Debug.Log("Unknown Card Type!");
                cardNumb = -1;
                break;
        }

        return cardNumb;
    }

    public int getPlayableSlot()
    {
        int slotNumb = -1;

        switch (playableSlots)
        {
            case PlayableSlot.Head:
                slotNumb = 0;
                break;
            case PlayableSlot.Chest:
                slotNumb = 1;
                break;
            case PlayableSlot.Hand:
                slotNumb = 2;
                break;
            case PlayableSlot.Feet:
                slotNumb = 3;
                break;
            default:
                Debug.Log("Unknown Card Slot!");
                slotNumb = -1;
                break;
        }

        return slotNumb;
    }
}
