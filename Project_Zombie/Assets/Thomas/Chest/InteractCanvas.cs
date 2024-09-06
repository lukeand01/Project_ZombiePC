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
    [SerializeField] protected GameObject titleHolder;
    [SerializeField] TextMeshProUGUI titleText;
    //while the thing is showing i want the  interact button if it has a border to keep slowly rotating


    private void Awake()
    {
        mainCam = Camera.main;

        if (priceHolder != null) priceHolder.SetActive(false);

        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.LookAt(mainCam.transform.position);
    }

    public virtual void ControlInteractButton(bool isVisible)
    {
        if (interatButtonHolder == null) return;
        if (interactButtonText == null) return;


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
        if(titleHolder == null)
        {
            UnityEngine.Debug.Log("this does not have title holder " + gameObject.name);
            return;
        }
        titleHolder.SetActive(false);
        priceHolder.SetActive(true);
        priceText.text = price.ToString();
    }



    public void ControlNameHolder(string name)
    {
        if (priceHolder == null) return;
        if (isDestroyed) return;
        titleHolder.SetActive(true);
        priceHolder.SetActive(false);
        titleText.text = name;
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

        if (price == 0)
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



    [Separator("NEW SYSTEM - FOR CITY")]
    [SerializeField] ButtonInteractUnit[] interactButtonArray;


    public void UpdateInteractButton_NewSystem(int index, string inputString, string titleString, string levelString)
    {
        interactButtonArray[index].gameObject.SetActive(true);
        interactButtonArray[index].SetUp(inputString, titleString, levelString);

    }
    public void DisableInteratButton_NewSystem(int index)
    {
        interactButtonArray[index].gameObject.SetActive(false);
    }



    #region CHECK WHAT CHILDREN FROM INTERACT CANVAS IT IS


    public virtual InteractCanvas_ChestGun GetInteractChestGun() => null;

    #endregion


}







//we need to show the amount of points that it cost.