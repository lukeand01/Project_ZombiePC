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
    [SerializeField] GameObject holder;
    [SerializeField] Animator _animator;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI inputText;
    //[SerializeField] TextMeshProUGUI ammoText_Current;
    //[SerializeField] TextMeshProUGUI ammoText_Reserve;
    
    GunClass gun_Perma;
    GunClass gun_Temp;

    GunClass GetCurrentGun { get 
        {
            if (gun_Temp != null) 
            {
                if (gun_Temp.data != null && !gun_Temp.isEquipped) return gun_Temp;
            }
              
            if(gun_Perma != null)
            {
                if (gun_Perma.data != null && !gun_Perma.isEquipped) return gun_Perma;
            }
           
            return null;

        } }


    const string ANIMATION_NORMAL = "Normal";
    const string ANIMATION_HIGHLIGHTED = "Highlighted";
    const string ANIMATION_PRESSED = "Pressed";
    const string ANIMATION_DISABLED = "Disabled";




    private void Awake()
    {
        holder.SetActive(false);
        

    }

    private void Start()
    {
        
    }

    public void UpdateKeyText(int index)
    {


        if (index == 1)
        {
            string key = PlayerHandler.instance._playerController.key.GetKey(KeyType.SwapWeapon_1).ToString();
            inputText.text = key;
        }
        if (index == 2)
        {
            string key = PlayerHandler.instance._playerController.key.GetKey(KeyType.SwapWeapon_2).ToString();
            inputText.text = key;
        }
    }

    public void SetUp(GunClass gun_Perma, GunClass gun_Temp)
    {
        this.gun_Perma = gun_Perma;
        this.gun_Temp = gun_Temp;

        UpdateUI();

        
    }

 
    public void UpdateUI()
    {
        if(GetCurrentGun == null)
        {
            holder.SetActive(false);
            return;
        }

        icon.sprite = GetCurrentGun.data.itemIcon;
        Unselect();
        //ammoText_Current.text = GetCurrentGun.ammoCurrent.ToString();
        //ammoText_Reserve.text = GetCurrentGun.ammoReserve.ToString();
        holder.SetActive(true);
    }

    public void Select()
    {
        
    }
    public void Unselect()
    {


    }
    public void UpdateAmmo(int ammo_Current, int ammo_Reserve)
    {
        //ammoText_Current.text = ammoText_Current.ToString();
        //ammoText_Reserve.text = ammoText_Reserve.ToString();
    }

    bool isPause = false;
    bool isEnd = false;


    private void Update()
    {
       


        isPause = UIHandler.instance._pauseUI.IsPauseOn();
        isEnd = UIHandler.instance._EndUI.IsEnd();


     
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        if (isPause || isEnd)
        {
            Debug.Log("this");
            UIHandler.instance._pauseUI.DescribeGun(gun_Temp, transform);
        }
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        if (isPause || isEnd)
        {
            UIHandler.instance._pauseUI.StopDescription();
        }
    }
    private void OnDisable()
    {
        
    }



}
