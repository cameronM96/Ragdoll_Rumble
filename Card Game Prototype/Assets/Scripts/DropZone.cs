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
    public GameObject[] slot;
    public UIManager uiManager;

    private void Start()
    {
        uiManager = transform.parent.GetComponentInParent<UIManager>();
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
                    eventData.pointerDrag.GetComponent<CardDisplay>().PlayCard(characterBase, slot);
                    d.returnParent = this.transform;
                }
            }
        }
    }
}

