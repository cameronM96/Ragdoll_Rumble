using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumTypes;

public class CardDisplay : MonoBehaviour
{
    public Card card;

    public Text nameText;
    public Text description;
    public Text cardInfoText;

    public Image artworkImage;
    public Image background;
    public Image rarityImage;
    public Image[] rings = new Image[4];
    
    public void Initialise()
    {
        // Update Card Display
        if (card == null)
            return;

        nameText = this.transform.GetChild(0).GetComponent<Text>();
        description = this.transform.GetChild(4).GetComponent<Text>();
        artworkImage = this.transform.GetChild(1).GetComponent<Image>();
        cardInfoText = this.transform.GetChild(2).GetComponent<Text>();
        background = GetComponent<Image>();

        cardInfoText.text = "";

        nameText.text = card.name;
        if (card.description != "")
            description.text = card.description + "\n";

        if (card.attack != 0)
            cardInfoText.text += ("\nAttack: " + card.attack);
        if (card.armour != 0)
            cardInfoText.text += ("\nArmour: " + card.armour);
        if (card.hP != 0)
            cardInfoText.text += ("\nHealth: " + card.hP);
        if (card.speed != 0)
            cardInfoText.text += ("\nSpeed: " + card.speed);
        if (card.atkSpeed != 0)
            cardInfoText.text += ("\nAttack Speed: " + card.atkSpeed);

        artworkImage.sprite = card.artwork;
        background.sprite = card.background;

        for (int i = 0; i < rings.Length; i++)
            rings[i] = this.transform.GetChild(5).GetChild(i).GetChild(0).GetComponent<Image>();

        //update Rarity
        rarityImage = this.transform.GetChild(3).GetComponent<Image>();
        rarityImage.sprite = card.rarityImage;

        // Update all slots
        PlayableSlot currentSlot = PlayableSlot.None;
        for (int i = 0; i < rings.Length; i++)
        {
            switch (i)
            {
                case 0:
                    currentSlot = PlayableSlot.Head;
                    break;
                case 1:
                    currentSlot = PlayableSlot.Hand;
                    break;
                case 2:
                    currentSlot = PlayableSlot.Chest;
                    break;
                case 3:
                    currentSlot = PlayableSlot.Feet;
                    break;
                default:
                    currentSlot = PlayableSlot.None;
                    Debug.Log("Unknown Slot!");
                    break;
            }

            if (currentSlot == (currentSlot & card.playableSlots))
                rings[i].color = Color.red;
            else
                rings[i].color = Color.grey;
        }
    }

    public void PlayCard (Base_Stats characterBase, GameObject[] slotSpots)
    {
        //Debug.Log("Playing card: " + card.name);
        // Add base stats
        card.PlayCard(characterBase);

        // Instantiate items on slots
        if (card.item != null)
        {
            for (int i = 0; i < slotSpots.Length; i++)
            {
                Instantiate(card.item, slotSpots[i].transform);
                //Sort items here?
            }
        }

        // Update stats display
        CompleteCard(characterBase);
    }

    void CompleteCard (Base_Stats bStats)
    {
        bStats.UpdateStatDisplay();
        Destroy(this.gameObject);
    }
}