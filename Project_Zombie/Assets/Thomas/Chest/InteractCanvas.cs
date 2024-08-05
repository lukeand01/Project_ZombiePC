using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractCanvas : MonoBehaviour
{
    Camera mainCam;

    [SerializeField] GameObject interatButtonHolder;
    [SerializeField] TextMeshProUGUI interactButtonText;
    [SerializeField] GameObject priceHolder;
    [SerializeField] TextMeshProUGUI priceText;

    private void Awake()
    {
        mainCam = Camera.main;

        if(priceHolder != null ) priceHolder.SetActive(false);
    }

    private void Update()
    {
        transform.LookAt(mainCam.transform.position);
    }

    public void ControlInteractButton(bool isVisible)
    {
        if (isDestroyed) return;
        if (isVisible)
        {
            KeyClass keyClass = PlayerHandler.instance._playerController.key;
            interactButtonText.text = keyClass.GetKey(KeyType.Interact).ToString();
        }

        interatButtonHolder.SetActive(isVisible);

    }

    public void ControlPriceHolder(int price)
    {
        if (isDestroyed) return;
        priceHolder.SetActive(true);
        priceText.text = "Price: " + price.ToString();
    }

    public void ControlNameHolder(string name)
    {
        if (isDestroyed) return;
        priceHolder.SetActive(true);
        priceText.text = name;
    }

    


    bool isDestroyed;

    private void OnDestroy()
    {
        isDestroyed = true;
    }


    #region MERCHANT
    [Separator("MERCHANT")]
    [SerializeField] Image merchantHolder;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] Image iconImage;

    public void StartMerchant(int price, string _name, string _description, Sprite _icon, Color backgroundColor)
    {
        ControlInteractButton(true);

        if(price == 0)
        {
            priceHolder.SetActive(false);
        }
        else
        {
            ControlPriceHolder(price);
        }

        
        merchantHolder.gameObject.SetActive(true);

        merchantHolder.color = backgroundColor;

        nameText.text = _name;
        descriptionText.text = _description;
        iconImage.sprite = _icon;

    }
    
    public void StopMerchant()
    {
        ControlInteractButton(false);
        priceHolder.SetActive(false);
        merchantHolder.gameObject.SetActive(false);
    }




    #endregion

}


//we need to show the amount of points that it cost.