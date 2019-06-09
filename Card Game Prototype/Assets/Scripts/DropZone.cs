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
            //Debug.Log(d.card.playableSlots + " : " + typeOfItem);
            //Debug.Log(typeOfItem == (typeOfItem & d.card.playableSlots));

            //Debug.Log("Draggable item found");
            if (cardSlot)
            {
                //Debug.Log("Returning to cardbay");
                d.returnParent = this.transform;
            }
            else if (typeOfItem == (typeOfItem & d.card.playableSlots))
            {
                //Debug.Log("Placing card in " + this.name);
                eventData.pointerDrag.GetComponent<CardDisplay>().PlayCard(characterBase,slot);
                d.returnParent = this.transform;
            }
        }
    }
}

