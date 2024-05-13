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
    [SerializeField] float spacing;
    [SerializeField] float spacingY;

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

            if (currentLimit > limitPerLine)
            {
                currentLimit = 1;
                posY += (int)(spacingY - 100);
                posX = 0;
            }

            if (canControlSize)
            {
                item.localScale = size;
            }

            SetChildAlongAxis(item, 0, posX - padding.left);
            SetChildAlongAxis(item, 1, posY - padding.top);
            posX += (int)(itemWidth + spacing);
        }
    }

}
