using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PackDropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Pack_Draggable d = eventData.pointerDrag.GetComponent<Pack_Draggable>();
        if (d != null)
            d.transform.SetParent(transform);
    }
}
