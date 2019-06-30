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
    public Sprite rarityImage;

    public int attack;
    public int armour;
    public int hP;
    public float speed;
    public float atkSpeed;

    public SO_Ability ability;

    public GameObject item;
    [HideInInspector] [EnumFlags] public Organisation organisation = Organisation.None;

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

    public void PlayCard (Base_Stats bStats, GameObject[] slotSpots)
    {
        UpdateStats(bStats);

        if (ability != null)
            ability.LoadAbility(bStats, item);

        // Instantiate items on slots
        if (item != null)
        {
            if (slotSpots != null)
            {
                for (int i = 0; i < slotSpots.Length; i++)
                {
                    if (slotSpots[i] != null)
                    {
                        GameObject newItem = Instantiate(item);
                        newItem.transform.position = slotSpots[i].transform.position;
                        newItem.transform.rotation = slotSpots[i].transform.rotation;
                        newItem.transform.SetParent(slotSpots[i].transform);
                        //Organise the items on this slot if applicable
                        if (organisation != (organisation & Organisation.None))
                        {
                            //Debug.Log("Organisation Acticated!");
                            OrganiseSlot organise = slotSpots[i].GetComponent<OrganiseSlot>();
                            if (organise != null)
                            {
                                // Set position and parent
                                if (organisation == (organisation & Organisation.Position))
                                {
                                    //Debug.Log("Organisation: Position");
                                    GameObject newParent = organise.OrganisePosition(newItem);
                                    newItem.transform.position = newParent.transform.position;
                                    newItem.transform.SetParent(newParent.transform);
                                }

                                // Set Rotation
                                if (organisation == (organisation & Organisation.Rotation))
                                {
                                    //Debug.Log("Organisation: Rotation");
                                    Vector3 newRot = organise.OrganiseRotation();
                                    newItem.transform.Rotate(0, newRot.y, 0);
                                    newItem.transform.Rotate(newRot.x, 0, 0);
                                }

                                // Set Scale
                                if (organisation == (organisation & Organisation.Scale))
                                {
                                    //Debug.Log("Organisation: Scale");
                                    newItem.transform.localScale *= organise.OrganiseScale();
                                }
                            }
                        }
                        //Debug.Log("Organisation Not Needed!");
                    }
                    else
                        Debug.Log("Slot was not set!");
                }
            }
            else
                Debug.Log("Slots could not be found!");
        }

        bStats.UpdateStatDisplay();
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
