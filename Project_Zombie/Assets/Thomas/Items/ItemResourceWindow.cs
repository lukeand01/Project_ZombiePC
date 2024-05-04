using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemResourceWindow : MonoBehaviour
{
    //this is what we use to describe the item resource window. only item resource.

    GameObject holder;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }

    [SerializeField]Image icon;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI quantityText;
    [SerializeField] TextMeshProUGUI tierText;

    public void OpenWindow(ItemClass item, Transform posRef)
    {
        //i need ref
        return;
        if (item.data == null) return;

        holder.SetActive(true);

        UpdatePosition(posRef);

        icon.sprite = item.data.itemIcon;
        nameText.text = item.data.itemName;
        descriptionText.text = item.data.itemDescription;
        quantityText.text = item.quantity.ToString();
        tierText.text = item.data.tierType.ToString();

    }

    void UpdatePosition(Transform posRef)
    {
        Vector3 offset = Vector3.zero;

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(posRef.position);

        Debug.Log("screenpos y " + screenPosition.y);
        Debug.Log("screen height " + Screen.height);

        if(screenPosition.y > Screen.height * 0.8f)
        {
            Debug.Log("bigger than height");
        }




        Debug.Log("this was the offset " + offset);

        holder.transform.position = posRef.position + offset;
    }

    public void CloseWindow()
    {
        holder.SetActive(false);
    }


}
