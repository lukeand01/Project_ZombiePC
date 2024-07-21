using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OwnedGunShowUnit : ButtonBase
{


    [Separator("OWNED GUN PART")]
    [SerializeField] Image icon;
    [SerializeField] GameObject selected_ForShow;
    [SerializeField] GameObject select_ForPause;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] GameObject holder;
    [SerializeField] Image chargeImage;
    GunClass gun;


    

    private void Awake()
    {
        holder.SetActive(false);

        chargeImage.fillAmount = 0;
    }
    public void SetUp(GunClass gun)
    {
        this.gun = gun;
        icon.sprite = gun.data.itemIcon;
        Unselect();
        ammoText.text = gun.ammoCurrent.ToString();
        holder.SetActive(true);
    }

    public void Select()
    {
        selected_ForShow.SetActive(true);
    }
    public void Unselect()
    {

        selected_ForShow.SetActive(false);
    }
    public void UpdateAmmo(int ammo)
    {
        ammoText.text = ammo.ToString();    
    }

    bool isPause = false;
    bool isEnd = false;


    private void Update()
    {
       


        isPause = UIHandler.instance._pauseUI.IsPauseOn();
        isEnd = UIHandler.instance._EndUI.IsEnd();

        if (!isPause && !isEnd)
        {
            select_ForPause.SetActive(false);
        }


      

    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);





        if (isPause || isEnd)
        {
            Debug.Log("this");
            select_ForPause.SetActive(true);
            UIHandler.instance._pauseUI.DescribeGun(gun, transform);
        }
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        if (isPause || isEnd)
        {
            select_ForPause.SetActive(false);
            UIHandler.instance._pauseUI.StopDescription();
        }
    }
    private void OnDisable()
    {
        select_ForPause.SetActive(false);
    }


    public void UpdateChargeImage(float current, float total)
    {
        chargeImage.fillAmount = current / total;
    }

}
