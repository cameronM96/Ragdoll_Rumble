using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganiseSlot : MonoBehaviour
{
    public Vector3 rotIncrement = new Vector3(20, 60, 0);
    public float scaleIncrement = 0.2f;
    private Vector3 currentRot;
    private int dropIteration;
    private int currentIteration;
    private bool first;
    private bool firstX;
    private bool flipFlop;
    private GameObject nextPosition;
    private float newScale;

    private void Start()
    {
        // might need to add one
        dropIteration = Mathf.RoundToInt(360 / rotIncrement.y);
        // might need to subtract one
        currentIteration = dropIteration;
        currentRot = Vector3.zero;
        first = true;
        flipFlop = false;
        firstX = true;
        nextPosition = this.gameObject;
        newScale = 1f;
    }

    public Vector3 OrganiseRotation()
    {
        // Rotate around y
        if (!first)
        {
            // Alternate between sides
            if (flipFlop)
            {
                //Debug.Log("Rotate y");
                currentRot.y -= 180;
                if (!firstX)
                    currentRot.y += rotIncrement.y;
            }
            else
            {
                //Debug.Log("Spin y");
                currentRot.y += 180;
                if (firstX)
                    firstX = false;
            }

            // Rotate around x
            if (currentIteration > dropIteration)
            {
                //Debug.Log("Drop x");
                currentIteration = 1;
                currentRot.x += rotIncrement.x;
                if (currentRot.y >= 360)
                    currentRot.y -= 360;
                firstX = true;
                flipFlop = true;
            }
        }
        else
        {
            first = false;
        }

        flipFlop = !flipFlop;
        ++currentIteration;

        //Debug.Log(currentIteration);
        //Debug.Log(currentRot);
        return currentRot;
    }

    public GameObject OrganisePosition(GameObject item)
    {
        GameObject newPosition = nextPosition;
        for (int i = 0; i < item.transform.childCount; i++)
        {
            if (item.transform.GetChild(i).tag == "NextPosition")
                nextPosition = item.transform.GetChild(i).gameObject;
        }

        return newPosition;
    }

    public float OrganiseScale()
    {
        float nextScale = newScale;
        newScale += scaleIncrement;
        return nextScale;
    }
}
