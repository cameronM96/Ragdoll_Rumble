using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using EnumTypes;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [EnumFlags] public PlayableSlot typeOfItem = PlayableSlot.None;
    public bool cardSlot = false;
    public Base_Stats characterBase;
    public bool rightSide = false;
    public GameObject[] slot;
    public UIManager uiManager;

    private void Start()
    {
        uiManager = transform.parent.GetComponentInParent<UIManager>();
        characterBase = transform.root.GetComponent<UIManager>().myCameraController.myPlayer.GetComponent<Base_Stats>();

        switch (typeOfItem)
        {
            case PlayableSlot.None:
                break;
            case PlayableSlot.Head:
                slot[0] = characterBase.slots[0];
                break;
            case PlayableSlot.Chest:
                slot[0] = characterBase.slots[1];
                break;
            case PlayableSlot.Hand:
                if (rightSide)
                    slot[0] = characterBase.slots[2];
                else
                    slot[0] = characterBase.slots[3];
                break;
            case PlayableSlot.Feet:
                slot[0] = characterBase.slots[4];
                slot[1] = characterBase.slots[5];
                break;
            default:
                break;
        }
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null)
        {
            d.placeHolderParent = this.transform;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null && d.placeHolderParent == this.transform)
        {
            d.placeHolderParent = d.returnParent;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("Dropping card: " + eventData.pointerDrag.GetComponent<CardDisplay>().card.name);
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null)
        {
            // If card bay, drop card
            if (cardSlot)
            {
                //Debug.Log("Returning to cardbay");
                d.returnParent = this.transform;
            }
            // Check if slot type matches card slot type (check if it can be dropped in this slot)
            else if (typeOfItem == (typeOfItem & d.card.playableSlots))
            {
                // Check if player is allowed to place any more this round
                if (uiManager.PlayCard())
                {
                    d.card.PlayCard(characterBase, slot);
                    d.returnParent = this.transform;

                    // Tell camera I am done being dragged
                    d.cameraController.draggingSomething = false;

                    // Destroys the card
                    eventData.pointerDrag.GetComponent<CardDisplay>().PlayCard();
                }
            }
        }
    }
}

