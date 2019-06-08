using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Draggable.Slot typeOfItem = Draggable.Slot.CHEST;
    public bool cardSlot = false;
    public Base_Stats characterBase;
    public GameObject[] slot;

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
            //Debug.Log("Draggable item found");
            if (cardSlot)
            {
                //Debug.Log("Returning to cardbay");
                d.returnParent = this.transform;
            }
            else if (typeOfItem == d.typeOfItem)
            {
                //Debug.Log("Placing card in " + this.name);
                eventData.pointerDrag.GetComponent<CardDisplay>().PlayCard(characterBase,slot);
                d.returnParent = this.transform;
            }
        }
    }
}
