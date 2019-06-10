using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

[CreateAssetMenu(menuName = "New Card")]
public class Card : ScriptableObject
{
    [HideInInspector] public Rarity rarity = Rarity.None;
    [HideInInspector] public CardType currentCardType = CardType.None;
    [HideInInspector] [EnumFlags] public PlayableSlot playableSlots = PlayableSlot.None;

    public string cardName;
    public string description;

    public Sprite artwork;
    public Sprite background;

    public int attack;
    public int armour;
    public int hP;
    public float speed;
    public float atkSpeed;

    public SO_Ability ability;

    public GameObject item;

    //[HideInInspector]
    public GameObject templateCard;

    public GameObject CreateCard ()
    {
        if (templateCard == null)
            return null;

        GameObject newCard = Instantiate(templateCard);

        newCard.GetComponent<CardDisplay>().card = this;
        newCard.GetComponent<CardDisplay>().Initialise();

        return newCard;
    }

    public void PlayCard (Base_Stats bStats)
    {
        UpdateStats(bStats);

        if (ability != null)
            ability.LoadAbility(bStats);
    }

    void UpdateStats(Base_Stats bStats)
    {
        if (attack != 0)
            bStats.attack += attack;

        if (armour != 0)
            bStats.armour += armour;

        if (hP != 0)
            bStats.maxHP += hP;

        if (speed != 0)
            bStats.speed += speed;

        if (atkSpeed != 0)
            bStats.atkSpeed += atkSpeed;
    }
}
