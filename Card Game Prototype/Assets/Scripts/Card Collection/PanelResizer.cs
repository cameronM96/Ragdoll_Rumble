using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelResizer : MonoBehaviour
{
    public RectTransform myRect;
    public bool width = false;
    public bool height = true;
    public enum GroupType { GridLayoutGroup, HorizontalLayoutGroup, VerticalLayoutGroup};
    public GroupType groupType = GroupType.GridLayoutGroup;
    public float elementSize;

    private void Start()
    {
        myRect = this.GetComponent<RectTransform>();
    }

    public void Resize()
    {
        //Debug.Log("Resizing!");

        switch (groupType)
        {
            case GroupType.GridLayoutGroup:
                GroupLayout();
                break;
            case GroupType.HorizontalLayoutGroup:
                HorizontalLayout();
                break;
            case GroupType.VerticalLayoutGroup:
                VerticalLayout();
                break;
            default:
                break;
        }
    }

    private void GroupLayout()
    {
        float targetHeight = 0;
        float targetWidth = 0;

        if (width)
        {
            // Get size panel needs to be based on children
            GridLayoutGroup gLayout = GetComponent<GridLayoutGroup>();
            targetWidth = gLayout.padding.left;
            targetWidth += (gLayout.cellSize.x + gLayout.spacing.x) * (Mathf.CeilToInt((float)transform.childCount / gLayout.constraintCount));
            targetWidth -= gLayout.spacing.x;
            targetWidth += gLayout.padding.right;
            //if (transform.childCount % gLayout.constraintCount != 0)
            //    targetWidth += (gLayout.cellSize.x + gLayout.spacing.x);

            // Set size
            myRect.sizeDelta = new Vector2(targetWidth, 0);
        }

        if (height)
        {
            // Get size panel needs to be based on children
            GridLayoutGroup gLayout = GetComponent<GridLayoutGroup>();
            targetHeight = gLayout.padding.top;
            targetHeight += (gLayout.cellSize.y + gLayout.spacing.y) * (Mathf.CeilToInt((float)transform.childCount / gLayout.constraintCount));
            targetHeight -= (gLayout.spacing.y);
            targetHeight += gLayout.padding.bottom;
            //if (transform.childCount % gLayout.constraintCount != 0)
            //    targetHeight += (gLayout.cellSize.y + gLayout.spacing.y);

            // Set size
            //Debug.Log("Calculated Height: " + targetHeight + "\nRows: " + (Mathf.CeilToInt((float)transform.childCount / gLayout.constraintCount)) + 
            //    "\nChild Count: " + transform.childCount + "\nColumn Cap: " + gLayout.constraintCount);
            myRect.sizeDelta = new Vector2(0, targetHeight);
        }
    }

    private void HorizontalLayout()
    {
        float targetWidth = 0;

        // Get size panel needs to be based on children
        HorizontalLayoutGroup gLayout = GetComponent<HorizontalLayoutGroup>();
        targetWidth = gLayout.padding.left;
        targetWidth += ((elementSize + gLayout.spacing) * transform.childCount);
        targetWidth -= gLayout.spacing;
        targetWidth += gLayout.padding.right;
        //if (transform.childCount % gLayout.constraintCount != 0)
        //    targetWidth += (gLayout.cellSize.x + gLayout.spacing.x);

        // Set size
        myRect.sizeDelta = new Vector2(targetWidth, 0);
    }

    private void VerticalLayout()
    {
        float targetHeight = 0;

        // Get size panel needs to be based on children
        VerticalLayoutGroup gLayout = GetComponent<VerticalLayoutGroup>();
        targetHeight = gLayout.padding.top;
        targetHeight += ((elementSize + gLayout.spacing) * transform.childCount);
        targetHeight -= (gLayout.spacing);
        targetHeight += gLayout.padding.bottom;
        //if (transform.childCount % gLayout.constraintCount != 0)
        //    targetHeight += (gLayout.cellSize.y + gLayout.spacing.y);

        // Set size
        //Debug.Log("Calculated Height: " + targetHeight + "\nRows: " + (Mathf.CeilToInt((float)transform.childCount / gLayout.constraintCount)) + 
        //    "\nChild Count: " + transform.childCount + "\nColumn Cap: " + gLayout.constraintCount);
        myRect.sizeDelta = new Vector2(0, targetHeight);
    }
}
