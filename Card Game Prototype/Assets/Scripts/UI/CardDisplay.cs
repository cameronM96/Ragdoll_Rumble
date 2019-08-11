using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumTypes;

public class CardDisplay : MonoBehaviour
{
    // This script is used to create the cards the players make 
    // but does not contain functionality to play them.
    public Card card;

    public Text nameText;
    public Text description;
    public Text cardInfoText;

    public Image artworkImage;
    public Image background;
    public Image rarityImage;
    public Image[] rings = new Image[4];

    public Sprite[] statIcons;
    public GameObject iconTemplate;
    public Vector2 columnValue = new Vector2(10,35);
    public float yValue = -10f;
    public float rowValue = -20f;

    private bool reArrangeIcons;

    public void Initialise(Card newCard)
    {
        // Update Card Display
        card = newCard;
        reArrangeIcons = false;

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
            CreateIcon(0);
        if (card.armour != 0)
            CreateIcon(1);
        if (card.hP != 0)
            CreateIcon(2);
        if (card.speed != 0)
            CreateIcon(3);
        if (card.atkSpeed != 0)
            CreateIcon(4);

        if (reArrangeIcons)
        {
            Debug.Log("Rearranging");
            cardInfoText.GetComponent<GridLayoutGroup>().enabled = false;
            //float currentY = yValue;
            //bool flipFlop = false;
            //foreach (Transform child in cardInfoText.transform)
            //{
            //    if (!flipFlop)
            //    {
            //        child.localPosition = new Vector3(columnValue.x, currentY);
            //    }
            //    else
            //    {
            //        child.localPosition = new Vector3(columnValue.y, currentY);
            //        currentY += rowValue;
            //    }
            //}

            // Center objects that are solo on a new row
            if (cardInfoText.transform.childCount == 1)
            {
                RectTransform reSetSize = cardInfoText.transform.GetChild(0).GetComponent<RectTransform>();
                reSetSize.sizeDelta = new Vector2(20, 20);
            }
            else
            {
                int lastChild = cardInfoText.transform.childCount;
                --lastChild;
                Transform centerObject = cardInfoText.transform.GetChild(lastChild);
                Vector3 newPos = centerObject.transform.localPosition;
                newPos.x = 
                    (cardInfoText.transform.GetChild(lastChild - 1).transform.position.x +
                    cardInfoText.transform.GetChild(lastChild - 2).transform.position.x) / 2;
                centerObject.localPosition = newPos;
            }

            cardInfoText.text = "";
            description.text = card.abilityDescription;
        }
        else
            cardInfoText.text = card.abilityDescription;

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

    public void PlayCard ()
    {
        //Debug.Log("Playing card: " + card.name);
        // Add base stats
        //card.PlayCard(characterBase, slotSpots);

        Destroy(this.gameObject);
    }

    private void CreateIcon(int value)
    {
        //reArrangeIcons = !reArrangeIcons;

        GameObject newIcon = Instantiate(iconTemplate, cardInfoText.transform);
        newIcon.GetComponent<Image>().sprite = statIcons[value];
        float statValue = 0;
        Color iconColour = Color.grey;
        switch (value)
        {
            case 0:
                statValue = card.attack;
                iconColour = Color.red;
                break;
            case 1:
                statValue = card.armour;
                iconColour = Color.yellow;
                break;
            case 2:
                statValue = card.hP;
                iconColour = Color.green;
                break;
            case 3:
                statValue = card.speed;
                iconColour = Color.blue;
                break;
            case 4:
                statValue = card.atkSpeed;
                iconColour = Color.magenta;
                break;
        }

        newIcon.GetComponentInChildren<Text>().text = statValue.ToString();
        newIcon.GetComponent<Image>().color = iconColour;
    }
}