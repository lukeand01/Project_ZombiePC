using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractCanvas : MonoBehaviour
{
    Camera mainCam;

    [SerializeField] GameObject interatButtonHolder;
    [SerializeField] TextMeshProUGUI interactButtonText;



    private void Awake()
    {
        mainCam = Camera.main;
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


    bool isDestroyed;

    private void OnDestroy()
    {
        isDestroyed = true;
    }
}


//we need to show the amount of points that it cost.