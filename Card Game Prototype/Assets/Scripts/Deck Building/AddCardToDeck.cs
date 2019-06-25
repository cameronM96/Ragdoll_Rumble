using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using EnumTypes;

public class AddCardToDeck : MonoBehaviour
{
    public Text quantityText;
    public Toggle canCraft;
    private Card gameCard;

    private void Awake()
    {
        canCraft = GetComponent<Toggle>();
    }

    public void SetGameCard(Card card)
    {
        gameCard = card;
    }

    private void OnMouseDown()
    {
        canCraft = GetComponent<Toggle>();
        Card card = gameCard;
        if (card == null)
        {
            return;
        }

        if (!canCraft)
        {
            if (CardCollection.Instance.quantityOfEachCard[card] - DeckBuildingScreen.Instance.builderScript.NumberOfThisCardInDeck(gameCard) > 0)
            {
                DeckBuildingScreen.Instance.builderScript.AddCard(card);
                UpdateQuantity();
            }
            else
            {
                //not enough cards add a pop up or somthing here later
            }
        }
        else
        {
            if (CraftingScreen.Instance.Visible)
            {
                return;
            }

            Ray clickPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitPoint;

            if (Physics.Raycast(clickPoint, out hitPoint))
            {
                if (hitPoint.collider == this.GetComponent<Collider>())
                {
                    Debug.Log("Crafting mode for " + this.name);
                    CraftingScreen.Instance.ShowCraftingScreen(GetComponent<Card>());
                }
            }
        }
    }

    public void UpdateQuantity()
    {
        int quantity = CardCollection.Instance.quantityOfEachCard[gameCard];

        if (DeckBuildingScreen.Instance.builderScript.InDeckBuildingMode && DeckBuildingScreen.Instance.showReducedQuantitiesInDeckBuilding)
        {
            quantity -= DeckBuildingScreen.Instance.builderScript.NumberOfThisCardInDeck(gameCard);
        }

        quantityText.text = ("X " + quantity.ToString());
    }

}
