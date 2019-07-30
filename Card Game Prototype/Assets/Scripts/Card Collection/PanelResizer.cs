using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelResizer : MonoBehaviour
{
    public RectTransform myRect;
    public bool width = false;
    public bool height = true;

    private void Start()
    {
        myRect = this.GetComponent<RectTransform>();
    }

    public void Resize()
    {
        float targetHeight = 0;
        float targetWidth = 0;

        if (width)
        {
            // Get size panel needs to be based on children
            GridLayoutGroup gLayout = GetComponent<GridLayoutGroup>();
            targetWidth = gLayout.padding.left;
            targetWidth += (gLayout.cellSize.x + gLayout.spacing.x) * (transform.childCount / gLayout.constraintCount);

            if (transform.childCount % gLayout.constraintCount != 0)
                targetWidth += (gLayout.cellSize.x + gLayout.spacing.x);

            // Set size
            myRect.sizeDelta = new Vector2(targetWidth, 0);
        }

        if (height)
        {
            // Get size panel needs to be based on children
            GridLayoutGroup gLayout = GetComponent<GridLayoutGroup>();
            targetHeight = gLayout.padding.top;
            targetHeight += (gLayout.cellSize.y + gLayout.spacing.y) * (transform.childCount / gLayout.constraintCount);

            if (transform.childCount % gLayout.constraintCount != 0)
                targetHeight += (gLayout.cellSize.y + gLayout.spacing.y);

            // Set size
            myRect.sizeDelta = new Vector2(0, targetHeight);
        }
        
    }
}
