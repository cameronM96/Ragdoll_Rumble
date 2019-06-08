using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;

    public Text nameText;
    public Text cardInfoText;

    public Image artworkImage;
    public Image background;

    // Start is called before the first frame update
    void Start()
    {
        nameText = this.transform.GetChild(0).GetComponent<Text>();
        artworkImage = this.transform.GetChild(1).GetComponent<Image>();
        cardInfoText = this.transform.GetChild(2).GetComponent<Text>();
        background = GetComponent<Image>();

        nameText.text = card.name;
        if (card.description != "")
            cardInfoText.text = card.description + "\n";

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
                Instantiate(card.item[i], slotSpots[i].transform);
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