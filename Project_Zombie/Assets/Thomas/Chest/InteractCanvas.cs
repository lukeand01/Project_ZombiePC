using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

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
}


//we need to show the amount of points that it cost.