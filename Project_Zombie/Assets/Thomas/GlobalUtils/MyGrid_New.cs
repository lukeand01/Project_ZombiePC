using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyGrid_New : LayoutGroup
{


    [Separator("SIZE")]
    [SerializeField] bool canControlSize;
    [ConditionalField(nameof(canControlSize))][SerializeField] Vector3 size = Vector3.one;

    [Separator("SPACING")]
    [SerializeField] float spacingX;
    [SerializeField] float spacingY;

    [Separator("SPACING BASED IN SCREEN SIZE")]
    [SerializeField] bool shouldUseScreenSpacing;
    [ConditionalField(nameof(shouldUseScreenSpacing))][SerializeField] float spacingBasedInPercentScreenWidth = 0.1f;
    [ConditionalField(nameof(shouldUseScreenSpacing))][SerializeField] float spacingBasedInPercentScreenHeight = 0.1f;

    [Separator("Limit")]
    public int limitPerLine = 3;


    //i will use this to force things to work correctly.


    public override void CalculateLayoutInputVertical()
    {
        UpdateGrid();
    }

    public override void SetLayoutHorizontal()
    {
    }

    public override void SetLayoutVertical()
    {
    }

    protected override void OnTransformChildrenChanged()
    {
        base.OnTransformChildrenChanged();
        //UpdateGrid();
    }


    void UpdateGrid()
    {
        int currentLimit = 0;

        int posX = 0;
        int posY = 0;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            var item = rectChildren[i];
            float itemWidth = item.sizeDelta.x;
            currentLimit++;

            int spacingY_Calculated = 0;
            int spacingX_Calculated = 0;


            if (shouldUseScreenSpacing)
            {
                spacingX_Calculated = (int)(Screen.width * spacingBasedInPercentScreenWidth);
                spacingY_Calculated = (int)(Screen.height * spacingBasedInPercentScreenHeight);
            }
            else
            {
                spacingY_Calculated = (int)(spacingY - 100);
                spacingX_Calculated = (int)(itemWidth + spacingX);
            }


            if (currentLimit > limitPerLine)
            {
                currentLimit = 1;
                posY += spacingY_Calculated;
                posX = 0;
            }

            if (canControlSize)
            {
                item.localScale = size;
            }

            
           
            SetChildAlongAxis(item, 0, posX - padding.left);
            SetChildAlongAxis(item, 1, posY - padding.top);


            posX += spacingX_Calculated;

        }
    }

    //the spacingX should be based in screen widhg and height

}
