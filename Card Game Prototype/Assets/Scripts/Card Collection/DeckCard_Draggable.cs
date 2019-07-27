﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckCard_Draggable : GenericDrag, IEndDragHandler, IBeginDragHandler
{
    public CollectionManager cM;
    public Collection_Card collectionCard;
    public Transform returnParent;

    public float timeToMove = 2f;
    public Vector3 startPosition;
    private float currentTime = 0f;
    private bool movingToDeck = true;
    private bool initialised = false;

    private void Awake()
    {
        cM = transform.root.GetComponent<CollectionManager>();
    }

    private void Update()
    {
        //if (initialised) {
        //    if (movingToDeck)
        //    {
        //        if (currentTime <= timeToMove)
        //        {
        //            currentTime += Time.deltaTime;
        //            transform.localPosition = Vector3.Lerp(startPosition, Vector3.zero, currentTime / timeToMove);
        //        }
        //        else
        //            movingToDeck = false;
        //    }
        //}
    }

    public void Initialise(Collection_Card cc, Transform parent, Vector3 pos)
    {
        this.transform.SetParent(parent);
        this.transform.SetAsFirstSibling();
        collectionCard = cc;
        //this.transform.position = pos;
        //startPosition = this.transform.localPosition;
        cM = transform.root.GetComponent<CollectionManager>();
        initialised = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        movingToDeck = false;
        returnParent = null;
        //this.transform.SetParent(null);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        movingToDeck = false;
        if (returnParent == null)
        {
            if (collectionCard != null)
                collectionCard.UpdateCardCount(++collectionCard.currentCardCount);

            cM.deckSaved = false;
            Destroy(this.gameObject);
        }
        else
        {
            this.transform.SetParent(null);
            this.transform.SetParent(returnParent);
            this.transform.SetAsFirstSibling();
        }
    }
}
